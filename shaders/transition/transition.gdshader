shader_type canvas_item;

uniform float screen_width = 1920;
uniform float screen_height = 1080;
uniform vec2 center = vec2(0.5f);
uniform vec2 playerCoord = vec2(0.5f);

uniform float progress : hint_range(0,1);
uniform float pixel_size = 15.0f;
uniform float circle_size : hint_range(0,2) = 1.5f;
uniform float curtains : hint_range(0,1) = 1.0f;

uniform int type : hint_range(0,5) = 0;

void fragment() {
	//Tipo Diamante Pixel
	if (type == 1) {
		float xFraction = fract(FRAGCOORD.x / pixel_size);
		float yFraction = fract(FRAGCOORD.y / pixel_size);

		float xDistance = abs(xFraction - 0.5);
		float yDistance = abs(yFraction - 0.5);

		if (xDistance + yDistance + UV.x + UV.y > progress * 4.0f) {
			discard;
		}
	}
	//Tipo Circulo na cordenada específica
	else if (type == 2) {
		float ratio = screen_width / screen_height;
		if (distance(playerCoord, vec2(mix(playerCoord.x, UV.x, ratio), UV.y)) < circle_size * (1.0 - progress)) {
			COLOR.a = 0.0f;
		}
	}
	//Tipo Circulo no Centro
	else if (type == 3) {
		float ratio = screen_width / screen_height;
		if (distance(center, vec2(mix(center.x, UV.x, ratio), UV.y)) < circle_size * (1.0 - progress)) {
			COLOR.a = 0.0f;
		}
	}
	//Tipo Corte Vertical
	else if (type == 4) {
		if (distance(vec2(0.5, 0.5), vec2(UV.x, UV.x)) < curtains * (1.0 - progress)) {
			COLOR.a = 0.0f;
		}
	}
	//Tipo Corte Horizontal
	else if (type == 5) {
		if (distance(vec2(0.5, 0.5), vec2(UV.y, UV.y)) < curtains * (1.0 - progress)) {
			COLOR.a = 0.0f;
		}
	}
}