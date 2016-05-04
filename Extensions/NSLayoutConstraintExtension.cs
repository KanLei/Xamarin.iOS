using System;
using UIKit;

public static class NSLayoutConstraintExtension
{
    private static NSLayoutConstraint[] VisualFormat(string format, NSLayoutFormatOptions formatOptions, params object[] viewsAndMetrics)
    {
        if (format == null)
        {
            throw new ArgumentNullException(nameof(format));
        }

        if (viewsAndMetrics == null)
        {
            throw new ArgumentNullException(nameof(viewsAndMetrics));
        }

        return NSLayoutConstraint.FromVisualFormat(format, formatOptions, viewsAndMetrics);
    }

    public static void AddConstraints(this UIView view, string visualFormat, NSLayoutFormatOptions formatOptions, params object[] viewsAndMetrics)
    {
        view.AddConstraints(VisualFormat(visualFormat, formatOptions, viewsAndMetrics));
    }
}
