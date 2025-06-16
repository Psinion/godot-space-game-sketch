shader_type spatial;
render_mode unshaded;

uniform sampler2D gradient;

varying vec3 objectPosition;

void vertex() {
	objectPosition = VERTEX;
}

void fragment() {
	float vertical_factor = clamp((objectPosition.x + 0.5), 0.0, 1.0);
	vec2 gradientUV = vec2(vertical_factor, 0);

	// Считываем текстуру
	vec4 inputColor = texture(gradient, gradientUV);
    ALBEDO = inputColor.rgb;
    ALPHA = inputColor.a;
}