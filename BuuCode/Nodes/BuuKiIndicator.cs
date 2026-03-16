using Godot;

namespace Buu.BuuCode.Nodes;

/// <summary>
/// Control that displays current Ki. Copy of Watcher-style indicator for now; replace art as needed.
/// Shows "0" until wired to combat: instantiate this scene in combat UI (e.g. via patch) and
/// update the script to read KiPower from the local player when the game API is confirmed.
/// </summary>
public partial class BuuKiIndicator : Control
{
	private Label? _label;

	public override void _Ready()
	{
		_label = GetNodeOrNull<Label>("%Label");
		if (_label != null)
			_label.Text = "0";
	}
}
