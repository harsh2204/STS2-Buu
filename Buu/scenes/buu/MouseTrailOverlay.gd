extends Control

@export var trail_length: int = 28
@export var trail_width: float = 22.0
@export var smoothing: float = 0.28
@export var base_color: Color = Color(1.0, 0.33, 0.78, 0.34)

var _points: Array[Vector2] = []


func _ready() -> void:
    mouse_filter = Control.MOUSE_FILTER_IGNORE
    set_process(true)


func _process(delta: float) -> void:
    var current_mouse_position := get_local_mouse_position()
    if _points.is_empty():
        _points.append(current_mouse_position)
    else:
        var blend := clamp(1.0 - pow(1.0 - smoothing, delta * 60.0), 0.0, 1.0)
        var smoothed_point := _points[0].lerp(current_mouse_position, blend)
        _points.insert(0, smoothed_point)

    while _points.size() > trail_length:
        _points.pop_back()

    queue_redraw()


func _draw() -> void:
    if _points.size() < 2:
        return

    var segment_count := _points.size() - 1
    for index in range(segment_count):
        var t := float(index) / float(max(1, segment_count))
        var width := lerp(trail_width, 2.0, t)
        var alpha_falloff := (1.0 - t) * (1.0 - t)
        var segment_color := Color(base_color.r, base_color.g, base_color.b, base_color.a * alpha_falloff)
        draw_line(_points[index], _points[index + 1], segment_color, width, true)

    var head := _points[0]
    draw_circle(head, trail_width * 0.62, Color(base_color.r, base_color.g, base_color.b, base_color.a * 0.48))
    draw_circle(head, trail_width * 0.34, Color(1.0, 0.70, 0.93, base_color.a * 0.60))
