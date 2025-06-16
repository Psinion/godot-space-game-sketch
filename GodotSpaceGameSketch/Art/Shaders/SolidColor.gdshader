shader_type spatial;
render_mode unshaded;

uniform vec4 color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
    ALBEDO = color.rgb;
    ALPHA = color.a;
}