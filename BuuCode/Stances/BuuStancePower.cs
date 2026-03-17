using BaseLib.Abstracts;
using BaseLib.Extensions;
using Buu.BuuCode.Extensions;
using Buu.BuuCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace Buu.BuuCode.Stances;

public abstract class BuuStancePower : BuuPower
{
    private Node2D? _vfxInstance;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;
    protected override bool IsVisibleInternal => false;

    protected virtual string? AuraScenePath => null;

    /// <summary>Path to the PackedScene for this stance's combat visual (e.g. res://Buu/animation/buu_node_majin.tscn).</summary>
    protected virtual string? StanceVisualScenePath => null;

    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => CustomPackedIconPath;

    private const string RegularVisualPath = "res://Buu/animation/buu_node.tscn";

    public virtual async Task OnEnterStance(Creature owner)
    {
        await CreateAura(owner);
        if (!string.IsNullOrEmpty(StanceVisualScenePath))
            ReplaceVisualsWithStanceScene(owner, StanceVisualScenePath);
    }

    public virtual async Task OnExitStance(Creature owner)
    {
        RemoveAura();
        ReplaceVisualsWithStanceScene(owner, RegularVisualPath);
        await Task.CompletedTask;
    }

    private static void ReplaceVisualsWithStanceScene(Creature owner, string scenePath)
    {
        var creatureNode = MegaCrit.Sts2.Core.Nodes.Rooms.NCombatRoom.Instance?.GetCreatureNode(owner);
        if (creatureNode == null || string.IsNullOrEmpty(scenePath)) return;

        var oldVisuals = creatureNode.GetNodeOrNull("Visuals") as Node2D;
        var position = oldVisuals?.Position ?? new Vector2(0, -19.805f);
        var scale = oldVisuals?.Scale ?? Vector2.One;

        oldVisuals?.QueueFree();

        var scene = GD.Load<PackedScene>(scenePath);
        if (scene == null) return;

        var newVisuals = scene.Instantiate<Node2D>();
        newVisuals.Name = "Visuals";
        newVisuals.Position = position;
        newVisuals.Scale = scale;
        creatureNode.AddChild(newVisuals);
    }

    private Task CreateAura(Creature owner)
    {
        var creatureNode = MegaCrit.Sts2.Core.Nodes.Rooms.NCombatRoom.Instance?.GetCreatureNode(owner);
        var visuals = creatureNode?.Visuals;
        if (visuals == null || string.IsNullOrEmpty(AuraScenePath)) return Task.CompletedTask;

        var container = visuals.GetNodeOrNull<Node2D>("StanceVfxContainer");
        if (container == null)
        {
            container = new Node2D { Name = "StanceVfxContainer", Position = Vector2.Zero };
            visuals.AddChild(container);
        }

        if (_vfxInstance != null && GodotObject.IsInstanceValid(_vfxInstance)) _vfxInstance.QueueFree();

        var scene = GD.Load<PackedScene>(AuraScenePath);
        if (scene == null) return Task.CompletedTask;

        _vfxInstance = scene.Instantiate<Node2D>();
        _vfxInstance.Position = Vector2.Zero;
        container.AddChild(_vfxInstance);
        _vfxInstance.Scale = Vector2.One;

        return Task.CompletedTask;
    }

    private void RemoveAura()
    {
        if (_vfxInstance == null || !GodotObject.IsInstanceValid(_vfxInstance)) return;
        _vfxInstance.QueueFree();
        _vfxInstance = null;
    }
}
