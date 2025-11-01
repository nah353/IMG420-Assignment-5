shader_type canvas_item;
// TODO: Add uniform variables for:
// - Time-based animation
// - Color gradient (hint: use multiple color uniforms)
// - Wave intensity
uniform float wave_intensity : hint_range(0.2, 1.0) = 0.2;

uniform vec4 color_start : source_color = vec4(1.0, 0.5, 0.0, 1.0);
uniform vec4 color_end : source_color = vec4(1.0, 0.0, 0.5, 1.0);

void fragment() {
// TODO: Implement wave distortion using UV and TIME
// TODO: Create color gradient based on UV.y
// TODO: Apply final color with alpha
// Starter hint:
// vec2 uv = UV;
// uv.x += sin(uv.y * 10.0 + TIME) * wave_intensity;
	vec2 uv = UV;
    uv.x += sin(uv.y * 20.0 + TIME * 3.0) * wave_intensity;

    vec4 tex_color = texture(TEXTURE, uv);
    vec4 gradient_color = mix(color_start, color_end, uv.y);
    COLOR = tex_color * gradient_color;
}