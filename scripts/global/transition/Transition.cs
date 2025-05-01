using Godot;
using System;

public partial class Transition : CanvasLayer
{
	[Signal] public delegate void StartedEventHandler();
	[Signal] public delegate void PeakEventHandler();
	[Signal] public delegate void FinishedEventHandler();

	private const string EndIn = "in";
	private const string EndOut = "out";
	private string _animationOut = null; // Animação de saída

    private ColorRect _color_rect; // Node da caixa preta
	private AnimationPlayer _animation; // Node da animação
	private ShaderMaterial _shaderMaterial; // Acesso ao shaders

	private Vector2? _customCoord = null; // Cordenada customizada para o shaders

	private bool _updadeCustomCoordInShader = false; // Controle se atualiza a posição do player no shaders ou não

	public override void _Ready()
	{

		_color_rect = GetNode<ColorRect>("color_rect");
		_animation = GetNode<AnimationPlayer>("animation");

		_shaderMaterial = (ShaderMaterial)_color_rect.Material;

		_color_rect.Visible = false;
		_animation.AnimationFinished += _on_animation_finished;
	}

    public override void _Process(double delta)
    {
        if (_updadeCustomCoordInShader && _customCoord != null)
			UpdateCustomCoordInShader((Vector2)_customCoord);
    }

	private void _on_animation_finished(StringName animationName)
	{ 
		// Função chamada ao finalizar a animação
		string animation = animationName.ToString();

		if (animation.EndsWith(EndIn))
		{
			EmitSignal("Peak");
            string animationOut = _animationOut ?? animation.Replace(EndIn, EndOut);


			if (animationOut == "circle_custom_coord_out")
			{
				// Atualiza a posição do shaders se a transição de saída for a circle_custom_coord
				_customCoord = Global.CheckPoint.GlobalPosition;
				_updadeCustomCoordInShader = true;
			}

            _animation.Play(animationOut);
			_animationOut = null;
			return;
		}

		if (animation.EndsWith(EndOut))
		{
			EmitSignal("Finished");
			_color_rect.Visible = false;
			_customCoord = null;
			_updadeCustomCoordInShader = false;
		}
	}
	public void Start ( string animationIn, string animationOut = null )
	{
		// Inicia o efeito
		EmitSignal("Started");
		_color_rect.Visible = true;

		if (animationIn == "circle_custom_coord" || animationOut == "circle_custom_coord")
		{
			Player player = Global.GetPlayer();
			_customCoord = player.GlobalPosition;
			_updadeCustomCoordInShader = true;
		};

		_animation.Play($"{animationIn}_{EndIn}");

		_animationOut = animationOut != null ? $"{animationOut}_{EndOut}" : null;
	}

	private void UpdateCustomCoordInShader(Vector2 globalPosition)
	{
		// Transforma a posição global da cordenada customizada em uma posição UV (0-1)
		// Isso é necessário pois o shaders trabalha com essa posição relativa

		Camera2D camera = GetViewport().GetCamera2D();
		Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
		
		// Obtém a transformação da câmera
		Vector2 cameraTransform = camera.GetScreenCenterPosition();
		Vector2 cameraPosition = cameraTransform;
		float cameraZoom = camera.Zoom.X;
		
		// Considerar o offset da câmera, se houver
		Vector2 cameraOffset = camera.Offset;
		cameraPosition += cameraOffset;
		
		// Calcula as bordas da viewport no espaço global
		Vector2 viewportHalfSize = viewportSize / 2 / cameraZoom;
		Vector2 viewportTopLeft = cameraPosition - viewportHalfSize;
		Vector2 viewportGlobalSize = viewportSize / cameraZoom;
		
		// Calcula a posição normalizada (UV)
		Vector2 uv = new(
			(globalPosition.X - viewportTopLeft.X) / viewportGlobalSize.X,
			(globalPosition.Y - viewportTopLeft.Y) / viewportGlobalSize.Y
		);
		
		// Verifica se está dentro dos limites válidos (0-1)
		uv.X = Mathf.Clamp(uv.X, 0f, 1f);
		uv.Y = Mathf.Clamp(uv.Y, 0f, 1f);

		// Envia ao shader
		_shaderMaterial.SetShaderParameter("customCoord", uv);
	}

	public bool IsRunning() => _animation.IsPlaying();
}
