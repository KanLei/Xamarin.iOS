using System;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreAnimation;

public static class ViewExtensions
{
	/// call in eg: layoutSubviews()
	public static void Round (this UIView view, UIRectCorner corners, nfloat radius)
	{
		var bezierPath = UIBezierPath.FromRoundedRect (view.Bounds,
		                                               corners,
		                                               new CGSize (radius, radius));
		var shapeLayer = new CAShapeLayer ();
		shapeLayer.Frame = view.Bounds;
		shapeLayer.Path = bezierPath.CGPath;
		view.Layer.Mask = shapeLayer;
	}
}
