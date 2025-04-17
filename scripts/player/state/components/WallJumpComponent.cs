using System;
using Godot;

public partial class WallJumpComponent : StatePlayer
{
    [Export] private float WallJumpHorizontalForce = 200.0f; // Força para lado oposto da parede
    [Export] private float JumpForce = -250.0f; // Força do pulo
	[Export] private float WallSlideSpeed = 30.0f; // Velocidade de deslizamento na parede
    [Export] private float BufferTime = 0.1f; // Buffer do pulo
    [Export] private float CoyoteTime = 0.1f; // Tempo após sair da parede que ainda pode pular
    [Export] private float LockTime = 0.15f; // Tempo que bloqueia movimento ao pular

    private float _bufferTimer = 0f; // Contador do buffer de input
    private float _coyoteTimer = 0f; // Contador do coyote time
    private float _lockTimer = 0f; // Contador do tempo que bloqueia movimeto ao pular

    private float _playerDirection; // Direção que o player está (-1 esquerda, 1 direita)
    private float _wallDirection; // Direção da parede (-1 esquerda, 1 direita)

    private bool _canWallJump = false; // Controle se pode pular
    private bool _isTransitioning = false; // Indica se está na fase de transição suave

	private RayCast2D _rayCast; // Ray cast para detectar a parede

    private Vector2 _velocity;

    public override void Init()
    {
        _rayCast = GetNode<RayCast2D>("ray_cast");
    }

    public override void Update(float delta)
    {
        _velocity = Player.Velocity;
		_playerDirection = FlipH ? -1 : 1;

        // Matém o ray cast apontado para frente
        _rayCast.Scale = new Vector2(
            Math.Abs(_rayCast.Scale.X) * _playerDirection,
            _rayCast.Scale.Y);

        // Reset flags quando tocar o chão
		if (Player.IsOnFloor())
		{
			IsOnWall = false;
            IsWallJumping = false;
            _canWallJump = false;

            _lockTimer = 0;
			return;
		}

        CheckWall();
        UpdadeTimers(delta);
        HandlWallJumpLogic();

        Player.Velocity = _velocity;
    }

    private void HandlWallJumpLogic()
    {
        // Só permite wall jump se estiver na parede ou dentro do coyote time
        // e se o botão de pulo foi pressionado recentemente (buffer)
        if (_canWallJump && _bufferTimer < BufferTime &&
        (IsOnWall || _coyoteTimer < CoyoteTime))
        {
            DoWallJump();
        }
    }

    private void UpdadeTimers(float delta)
    {
        // Apenas atualiza o buffer se o botão de pulo for pressionado
        if (PressedKeyJump)
            _bufferTimer = 0;
        else if (_bufferTimer < BufferTime)
            _bufferTimer += delta;

        // Timer do coyote
        if (_coyoteTimer < CoyoteTime)
            _coyoteTimer += delta;

        if (!IsWallJumping) return;

        // Timer que bloqueia o controle durante o wall jump
        _lockTimer += delta;

        if (_lockTimer >= LockTime)
        {
             IsWallJumping = false;
            _lockTimer = 0;
        }
    }

    private void CheckWall()
    {
        bool wasOnWall = IsOnWall;

        // Verifica se está tocando uma parede
        if (!_rayCast.IsColliding() || Direction.X == 0)
		{
			IsOnWall = false;

            // Se acabou de sair da parede, inicia o coyote time
            if (wasOnWall)
            {
                _coyoteTimer = 0;
                _canWallJump = true;
            }
			return;
		}

        // Obtém a posição mundial da colisão
        Vector2 collisionPoint = _rayCast.GlobalPosition + _rayCast.TargetPosition.Rotated(_rayCast.GlobalRotation);

        // Verifica se o tile sendo colidido permite wall jump
        Map map = Global.Manager.GetMap();
        if (!map.CheckTileCanWallJump(collisionPoint)) return;

        // Segue caso seja permitido wall jump nesta parede
		IsOnWall = true;
        _wallDirection = (collisionPoint.X < Player.GlobalPosition.X) ? -1 : 1;

        // Pode fazer wall jump enquanto estiver na parede
        _canWallJump = true;

		// Aplica deslizamento se estiver movendo para baixo
        if (_velocity.Y > 0)
		    _velocity.Y = Mathf.Min(_velocity.Y, WallSlideSpeed);
    }

    private void DoWallJump()
    {
        IsWallJumping = true;
        _canWallJump = false;

        _velocity.X = -_wallDirection * WallJumpHorizontalForce;
        _velocity.Y = JumpForce;
		
        // Reseta os timers para evitar wall jumps consecutivos
        _bufferTimer = BufferTime;
        _coyoteTimer = CoyoteTime;
    }
}
