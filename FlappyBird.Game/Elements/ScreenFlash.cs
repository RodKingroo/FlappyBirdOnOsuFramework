using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Layout;
using osu.Framework.Timing;
using osuTK;

namespace FlappyBird.Game.Elements;

public partial class ScreenFlash : Box
{
    public ScreenFlash()
    {
        Colour = Colour4.White;
        RelativeSizeAxes = Axes.Both;
        Alpha = 0.0f;

    }

    public override bool RemoveCompletedTransforms 
        => base.RemoveCompletedTransforms;

    public override bool DisposeOnDeathRemoval 
        => base.DisposeOnDeathRemoval;

    public override Vector2 Size 
    { 
        get => base.Size; 
        set 
        {
             if(value != base.Size) base.Size = value; 
        }
    }

    public override float Width 
    { 
        get => base.Width; 
        set 
        {
            if(value != base.Width) base.Width = value; 
        } 
    }
    
    public override float Height 
    { 
        get => base.Height; 
        set 
        {
            if(value != base.Height) base.Height = value;
        }  
    }
    public override Axes RelativeSizeAxes 
    { 
        get => base.RelativeSizeAxes; 
        set 
        {
            if(value != base.RelativePositionAxes) base.RelativeSizeAxes = value; 
        }
    }

    public override RectangleF BoundingBox 
        => base.BoundingBox;

    public override Anchor Origin 
    { 
        get => base.Origin; 
        set
        {
            if(value != base.Origin) base.Origin = value;
        }  
    }
    public override Vector2 OriginPosition 
    { 
        get => base.OriginPosition; 
        set
        {
            if(value != base.OriginPosition) base.OriginPosition = value; 
        } 
    }

    public override bool IsPresent => base.IsPresent;

    public override IFrameBasedClock Clock 
    { 
        get => base.Clock; 
        set
        { 
            if(value != base.Clock) base.Clock = value;
        } 
    
    }
    
    public override double LifetimeStart 
    { 
        get => base.LifetimeStart; 
        set
        {
            if(value != base.LifetimeStart) base.LifetimeStart = value; 
        } 
    }
    
    public override double LifetimeEnd { 
        get => base.LifetimeEnd; 
        set
        {
            if(value != base.LifetimeEnd) base.LifetimeEnd = value; 
        } 
    }

    public override bool RemoveWhenNotAlive 
        => base.RemoveWhenNotAlive;

    public override Quad ScreenSpaceDrawQuad 
        => base.ScreenSpaceDrawQuad;

    public override DrawInfo DrawInfo 
        => base.DrawInfo;

    public override DrawColourInfo DrawColourInfo 
        => base.DrawColourInfo;

    public override bool HandleNonPositionalInput 
        => base.HandleNonPositionalInput;

    public override bool HandlePositionalInput 
        => base.HandlePositionalInput;

    public override bool RequestsFocus 
        => base.RequestsFocus;

    public override bool AcceptsFocus 
        => base.AcceptsFocus;

    public override bool PropagateNonPositionalInputSubTree 
        => base.PropagateNonPositionalInputSubTree;

    public override bool PropagatePositionalInputSubTree 
        => base.PropagatePositionalInputSubTree;

    public override bool DragBlocksClick 
        => base.DragBlocksClick;

    public override Texture Texture 
    { 
        get => base.Texture; 
        set
        {
            if(value != base.Texture) base.Texture = value;
        }  
    }

    protected override Vector2 DrawScale 
        => base.DrawScale;

    protected override bool ShouldBeAlive 
        => base.ShouldBeAlive;

    public override void ApplyTransformsAt(double time, bool propagateChildren = false) 
        => base.ApplyTransformsAt(time: time, propagateChildren: propagateChildren);

    public override IDisposable BeginAbsoluteSequence(double newTransformStartTime, bool recursive = true) 
        => base.BeginAbsoluteSequence(newTransformStartTime: newTransformStartTime, recursive: recursive);

    public override void ClearTransforms(bool propagateChildren = false, string? targetMember = null) 
        => base.ClearTransforms(propagateChildren: propagateChildren, targetMember: targetMember);

    public override void ClearTransformsAfter(double time, bool propagateChildren = false, string? targetMember = null) 
        => base.ClearTransformsAfter(time: time, propagateChildren: propagateChildren, targetMember: targetMember);

    public override bool Contains(Vector2 screenSpacePos) 
        => base.Contains(screenSpacePos: screenSpacePos);

    public override bool Equals(object? obj) 
        => base.Equals(obj: obj);

    public override void FinishTransforms(bool propagateChildren = false, string? targetMember = null) 
        => base.FinishTransforms(propagateChildren: propagateChildren, targetMember: targetMember);

    public void Flash(double fadeInDuration, double fadeOutDuration) 
        => this.FadeIn(duration: fadeInDuration).Then().FadeOut(duration: fadeOutDuration);

    public override int GetHashCode() 
        => base.GetHashCode();

    public override void Hide() 
        => base.Hide();

    public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) 
        => base.ReceivePositionalInputAt(screenSpacePos: screenSpacePos);

    public override void Show() 
        => base.Show();

    public override string ToString() 
        => base.ToString();

    public override bool UpdateSubTree() 
        => base.UpdateSubTree();

    public override bool UpdateSubTreeMasking(Drawable source, RectangleF maskingBounds) 
        => base.UpdateSubTreeMasking(source: source, maskingBounds: maskingBounds);

    protected override Quad ComputeConservativeScreenSpaceDrawQuad() 
        => base.ComputeConservativeScreenSpaceDrawQuad();

    protected override bool ComputeIsMaskedAway(RectangleF maskingBounds) 
        => base.ComputeIsMaskedAway(maskingBounds: maskingBounds);

    protected override Quad ComputeScreenSpaceDrawQuad() 
        => base.ComputeScreenSpaceDrawQuad();

    protected override DrawNode CreateDrawNode() 
        => base.CreateDrawNode();

    protected override void Dispose(bool isDisposing) 
        => base.Dispose(isDisposing: isDisposing);

    protected override bool Handle(UIEvent uiEvent) 
        => base.Handle(e: uiEvent);

    protected override void InjectDependencies(IReadOnlyDependencyContainer dependencies) 
        => base.InjectDependencies(dependencies: dependencies);

    protected override void LoadAsyncComplete() 
        => base.LoadAsyncComplete();

    protected override void LoadComplete() 
        => base.LoadComplete();

    protected override bool OnClick(ClickEvent clickEvent) 
        => base.OnClick(e: clickEvent);

    protected override bool OnDoubleClick(DoubleClickEvent doubleClickEvent) 
        => base.OnDoubleClick(e: doubleClickEvent);

    protected override void OnDrag(DragEvent dragEvent) 
        => base.OnDrag(e: dragEvent);

    protected override void OnDragEnd(DragEndEvent dragEndEvent) 
        => base.OnDragEnd(e: dragEndEvent);

    protected override bool OnDragStart(DragStartEvent dragStartEvent) 
        => base.OnDragStart(e: dragStartEvent);

    protected override void OnFocus(FocusEvent focusEvent) 
        => base.OnFocus(e: focusEvent);

    protected override void OnFocusLost(FocusLostEvent focusLostEvent) 
        => base.OnFocusLost(e: focusLostEvent);

    protected override bool OnHover(HoverEvent hoverEvent) 
        => base.OnHover(e: hoverEvent);

    protected override void OnHoverLost(HoverLostEvent hoverLostEvent)
    {
        base.OnHoverLost(e: hoverLostEvent);
    }

    protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source) 
        => base.OnInvalidate(invalidation: invalidation, source: source);

    protected override bool OnJoystickAxisMove(JoystickAxisMoveEvent joystickAxisMove) 
        => base.OnJoystickAxisMove(e: joystickAxisMove);

    protected override bool OnJoystickPress(JoystickPressEvent joystickPressEvent) 
        => base.OnJoystickPress(e: joystickPressEvent);

    protected override void OnJoystickRelease(JoystickReleaseEvent joystickReleaseEvent) 
        => base.OnJoystickRelease(e: joystickReleaseEvent);

    protected override bool OnKeyDown(KeyDownEvent keyDownEvent) 
        => base.OnKeyDown(e: keyDownEvent);

    protected override void OnKeyUp(KeyUpEvent keyUpEvent) 
        => base.OnKeyUp(e: keyUpEvent);

    protected override bool OnMidiDown(MidiDownEvent midiDownEvent) 
        => base.OnMidiDown(e: midiDownEvent);

    protected override void OnMidiUp(MidiUpEvent midiUpEvent) 
        => base.OnMidiUp(e: midiUpEvent);

    protected override bool OnMouseDown(MouseDownEvent mouseDownEvent) 
        => base.OnMouseDown(e: mouseDownEvent);

    protected override bool OnMouseMove(MouseMoveEvent mouseMoveEvent) 
        => base.OnMouseMove(e: mouseMoveEvent);

    protected override void OnMouseUp(MouseUpEvent mouseUpEvent) 
        => base.OnMouseUp(e: mouseUpEvent);

    protected override bool OnScroll(ScrollEvent scrollEvent) 
        => base.OnScroll(e: scrollEvent);

    protected override void OnSizingChanged() 
        => base.OnSizingChanged();

    protected override bool OnTabletAuxiliaryButtonPress(TabletAuxiliaryButtonPressEvent tabletAuxiliaryButtonPressEvent) 
        => base.OnTabletAuxiliaryButtonPress(e: tabletAuxiliaryButtonPressEvent);

    protected override void OnTabletAuxiliaryButtonRelease(TabletAuxiliaryButtonReleaseEvent tabletAuxiliaryButtonReleaseEvent) 
        => base.OnTabletAuxiliaryButtonRelease(e: tabletAuxiliaryButtonReleaseEvent);

    protected override bool OnTabletPenButtonPress(TabletPenButtonPressEvent tabletPenButtonPressEvent) 
        => base.OnTabletPenButtonPress(e: tabletPenButtonPressEvent);

    protected override void OnTabletPenButtonRelease(TabletPenButtonReleaseEvent tabletPenButtonReleaseEvent) 
        => base.OnTabletPenButtonRelease(e: tabletPenButtonReleaseEvent);

    protected override bool OnTouchDown(TouchDownEvent touchDownEvent) 
        => base.OnTouchDown(e: touchDownEvent);

    protected override void OnTouchMove(TouchMoveEvent touchMoveEvent) 
        => base.OnTouchMove(e: touchMoveEvent);

    protected override void OnTouchUp(TouchUpEvent touchUpEvent) 
        => base.OnTouchUp(e: touchUpEvent);

    protected override void Update() 
        => base.Update();
}