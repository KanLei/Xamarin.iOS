using System;
using CoreGraphics;
using Foundation;
using UIKit;

public enum ScrollDirection
{
	Up = 1, Down = -1
}

[Register("ScrollText")]
public class ScrollText : UIView
{
	private UIButton firstButton;
	public EventHandler<string> Selected;

	public ScrollText (CGRect frame) : base (frame)
	{
		Initialize ();
	}

	public ScrollText (IntPtr handle):base(handle)
	{
		Initialize ();
	}

	public ScrollText ()
	{
		Initialize ();
	}

	/// <summary>
	/// 显示文本内容
	/// </summary>
	private string text;
	public string Text {
		get { return text; }
		set {
			if (String.IsNullOrWhiteSpace (value)) {
				return;
			}

			text = value;
			ScrollText ();
		}
	}

	/// <summary>
	/// 滚动方向
	/// </summary>
	public ScrollDirection Direction { get; set; } = ScrollDirection.Up;

	/// <summary>
	/// 滚动时间
	/// </summary>
	public nfloat Duration { get; set; } = 0.7f;

	/// <summary>
	/// 文本颜色
	/// </summary>
	public UIColor TextColor { get; set; } = UIColor.Black;

	/// <summary>
	/// 垂直对齐
	/// </summary>
	public UIControlContentVerticalAlignment VerticalAlignment { get; set; } = UIControlContentVerticalAlignment.Center;

	/// <summary>
	/// 水平对齐
	/// </summary>
	public UIControlContentHorizontalAlignment HorizontalAlignment { get; set; } = UIControlContentHorizontalAlignment.Left;

	private bool animating;
	private void ScrollText ()
	{
		if (animating)
			return;
		
		var secondButton = CreateButton (firstButton.Frame);
		Add (secondButton);

		var secondTranslation = CGAffineTransform.MakeTranslation (0, (int)Direction * secondButton.Frame.Height / 2);
		secondButton.Transform = CGAffineTransform.Scale (secondTranslation, 1.0f, 0.01f);

		UIView.Animate (Duration, () => {
			animating = true;
			var firstTranslation = CGAffineTransform.MakeTranslation (0, -((int)Direction) * firstButton.Frame.Height / 2);
			firstButton.Transform = CGAffineTransform.Scale (firstTranslation, 1.0f, 0.01f);
			secondButton.Transform = CGAffineTransform.MakeIdentity ();

		}, () => {
			firstButton.SetTitle (Text, UIControlState.Normal);
			firstButton.Transform = CGAffineTransform.MakeIdentity ();
			secondButton.RemoveFromSuperview ();
			animating = false;
		});
	}

	private void Initialize ()
	{
		ClipsToBounds = true;

		firstButton = CreateButton (this.Bounds);
		Add (firstButton);
	}

	private UIButton CreateButton (CGRect frame)
	{
		var button = new UIButton (frame);
		button.SetTitle (text, UIControlState.Normal);
		button.SetTitleColor (TextColor, UIControlState.Normal);
		button.HorizontalAlignment = HorizontalAlignment;
		button.VerticalAlignment = VerticalAlignment;
		button.TouchUpInside += (sender, e) => {
			Selected (this, button.Title(UIControlState.Normal));
		};
		return button;
	}
}

