using UIKit;

public static class WindowExtensions
{
	private static UIWindow popupWindow;  // 可重复使用同一个 Window

	public static void ShowPopupView (this UIView view)
	{
		popupWindow = popupWindow ?? CreatePopupWindow ();
		view.Frame = popupWindow.Bounds;
		popupWindow.AddSubview (view);
		popupWindow.MakeKeyAndVisible ();
	}

	public static void HidePopupView (this UIView view)
	{
		view.RemoveFromSuperview ();
		if (popupWindow != null) {
			popupWindow.Hidden = true;
			popupWindow.Dispose ();
			popupWindow = null;
		}
	}

	private static UIWindow CreatePopupWindow ()
	{
		var extWindow = new UIWindow (UIScreen.MainScreen.Bounds);
		extWindow.WindowLevel = UIWindowLevel.Alert;
		return extWindow;
	}
}

