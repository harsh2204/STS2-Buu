shader_type canvas_item;

uniform vec4 smoke_color : source_color = vec4(0.95, 0.28, 0.72, 1.0);
uniform float smoke_strength : hint_range(0.0, 1.0) = 0.42;
uniform float scale = 4.2;
uniform float speed = 0.09;
uniform float warp_strength : hint_range(0.0, 2.0) = 1.2;

float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

float noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    vec2 u = f * f * (3.0 - 2.0 * f);

    float a = hash(i);
    float b = hash(i + vec2(1.0, 0.0));
    float c = hash(i + vec2(0.0, 1.0));
    float d = hash(i + vec2(1.0, 1.0));

    return mix(mix(a, b, u.x), mix(c, d, u.x), u.y);
}

float fbm(vec2 p) {
    float value = 0.0;
    float amplitude = 0.5;
    for (int octave = 0; octave < 5; octave++) {
        value += amplitude * noise(p);
        p = p * 2.0 + vec2(13.4, 7.7);
        amplitude *= 0.5;
    }
    return value;
}

void fragment() {
    vec2 uv = UV;
    float time = TIME * speed;

    // Domain-warped FBM creates smoother smoke than hard threshold bands.
    vec2 p = uv * scale;
    vec2 warp = vec2(
        fbm(p * 0.9 + vec2(2.1, time * 1.1)),
        fbm(p * 1.1 + vec2(8.7, -time * 0.8))
    );
    p += (warp - vec2(0.5)) * warp_strength;

    vec2 drift = vec2(sin(uv.y * 7.0 + time * 5.0) * 0.18, -time * 2.6);
    float smoke_a = fbm(p + drift);
    float smoke_b = fbm((p + vec2(4.3, -2.7)) * 1.6 + drift * 1.35);
    float smoke_c = fbm((p + vec2(-3.1, 6.8)) * 2.1 + drift * 1.8);

    float smoke = smoke_a * 0.55 + smoke_b * 0.30 + smoke_c * 0.15;
    smoke = clamp(smoke * 1.22 - 0.22, 0.0, 1.0);

    // Soft masks to avoid visible hard edges in the overlay.
    float side_fade = smoothstep(0.02, 0.20, uv.x) * smoothstep(0.98, 0.80, uv.x);
    float vertical_fade = smoothstep(0.0, 0.75, uv.y);
    float alpha = pow(smoke, 1.9) * side_fade * vertical_fade * smoke_strength;

    COLOR = vec4(smoke_color.rgb, alpha);
}
