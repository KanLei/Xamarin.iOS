using System;

using UIKit;
using Foundation;
using System.Drawing;
using CoreGraphics;

namespace DemoApp
{
    /// <summary>
    /// 用于显示消息提示数量
    /// 使用介绍:约束 UIView 的 Top 和 Leading, 
    /// 设置其 Intrinsic Size 为 Placeholder 即可
    /// </summary>
    [Register("NotifyBadge")]
    public class NotifyBadge:UIView
    {
        const float M_PI = (float)Math.PI;
        CGSize contentSize = new CGSize(20, 20);

        public NotifyBadge(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        public NotifyBadge()
        {
            Initialize();
        }

        private string count;

        /// <summary>
        /// 提示消息数
        /// </summary>
        public string Count
        {
            get{ return count; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                count = value;
                AutoBadgeSizeWithString(count);
            }
        }

        /// <summary>
        /// 字体
        /// </summary>
        public UIFont Font{ get; set; }

        /// <summary>
        /// 文本颜色
        /// </summary>
        public UIColor TextColor{ get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public UIColor InsetColor{ get; set; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public UIColor FrameColor{ get; set; }

        /// <summary>
        /// 边框是否可见
        /// </summary>
        public bool FrameVisible{ get; set; }

        /// <summary>
        /// 是否呈现闪亮✨     的效果
        /// </summary>
        public bool Shining{ get; set; }

        /// <summary>
        /// 圆角
        /// </summary>
        public float CornerRoundness{ get; set; }

        /// <summary>
        /// 缩放比例
        /// </summary>
        public float ScaleFactor{ get; set; }

        private void Initialize()
        {
            ContentScaleFactor = UIScreen.MainScreen.Scale;
            BackgroundColor = UIColor.Clear;

            this.TextColor = UIColor.White;
            this.Font = UIFont.SystemFontOfSize(12);
            this.FrameVisible = false;
            this.FrameColor = UIColor.White;
            this.InsetColor = UIColor.Red;
            this.CornerRoundness = 0.4f;
            this.ScaleFactor = 0.95f;
            this.Shining = false;
        }

        public void AutoBadgeSizeWithString(string badgeString)
        {
            CGSize retValue;
            nfloat rectWidth, rectHeight;
            CGSize stringSize = new NSString(badgeString).GetSizeUsingAttributes(new UIStringAttributes{ Font = Font });
            nfloat flexSpace;
            if (badgeString.Length >= 2)
            {
                flexSpace = badgeString.Length;
                rectWidth = 15 + (stringSize.Width + flexSpace);
                rectHeight = 20;
                retValue = new CGSize(rectWidth * ScaleFactor, rectHeight * ScaleFactor);
            }
            else
            {
                retValue = new SizeF(20 * ScaleFactor, 20 * ScaleFactor);
            }
            contentSize = new CGSize(retValue.Width, retValue.Height);
            InvalidateIntrinsicContentSize();
            SetNeedsDisplay();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            using (var context = UIGraphics.GetCurrentContext())
            {
                DrawRoundedRect(rect, context);
                if (Shining)
                {
                    DrawShine(rect, context);
                }
                if (FrameVisible)
                {
                    DrawFrame(rect, context);
                }
                if (!string.IsNullOrWhiteSpace(Count))
                {
                    float sizeOfFont = 13.5f * ScaleFactor;
                    if (Count.Length < 2)
                    {
                        sizeOfFont += sizeOfFont * 0.20f;
                    }

                    var nsText = new NSString(Count);
                    var textFont = Font.WithSize(sizeOfFont);

                    var textSize = nsText.GetSizeUsingAttributes(new UIStringAttributes{ Font = textFont });
                    nsText.DrawString(new CGPoint((rect.Width / 2) - (textSize.Width / 2), 
                            rect.Height / 2 - textSize.Height / 2),
                        new UIStringAttributes{ Font = textFont, ForegroundColor = TextColor });
                }
            }
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        void DrawRoundedRect(CGRect rect, CGContext context)
        {
            context.SaveState();
            nfloat radius = rect.GetMaxY() * CornerRoundness;
            nfloat puffer = rect.GetMaxY() * 0.10f;
            nfloat maxX = rect.GetMaxX() - puffer;
            nfloat maxY = rect.GetMaxY() - puffer;
            nfloat minX = rect.GetMinX() + puffer;
            nfloat minY = rect.GetMinY() + puffer;

            context.BeginPath();
            context.SetFillColor(InsetColor.CGColor);
            context.AddArc(maxX - radius, minY + radius, radius, M_PI + (M_PI / 2f), 0f, false);
            context.AddArc(maxX - radius, maxY - radius, radius, 0, M_PI / 2f, false);
            context.AddArc(minX + radius, maxY - radius, radius, M_PI / 2f, M_PI, false);
            context.AddArc(minX + radius, minY + radius, radius, M_PI, M_PI + (M_PI / 2f), false);
            //            context.SetShadowWithColor (new SizeF (1, 1), 3, UIColor.Black.CGColor);
            context.FillPath();
            context.RestoreState();
        }

        /// <summary>
        /// 绘制闪亮的效果
        /// </summary>
        void DrawShine(CGRect rect, CGContext context)
        {
            context.SaveState();
            nfloat radius = rect.GetMaxY() * CornerRoundness;
            nfloat puffer = rect.GetMaxY() * 0.10f;
            nfloat maxX = rect.GetMaxX() - puffer;
            nfloat maxY = rect.GetMaxY() - puffer;
            nfloat minX = rect.GetMinX() + puffer;
            nfloat minY = rect.GetMinY() + puffer;

            context.BeginPath();
            context.AddArc(maxX - radius, minY + radius, radius, M_PI + (M_PI / 2f), 0f, false);
            context.AddArc(maxX - radius, maxY - radius, radius, 0, M_PI / 2f, false);
            context.AddArc(minX + radius, maxY - radius, radius, M_PI / 2f, M_PI, false);
            context.AddArc(minX + radius, minY + radius, radius, M_PI, M_PI + (M_PI / 2f), false);
            context.Clip();

            var locations = new nfloat[]{ 0f, 0.4f };
            var components = new nfloat[]{ 0.92f, 0.92f, 0.92f, 1.0f, 0.82f, 0.82f, 0.82f, 0.4f };
            using (var cspace = CGColorSpace.CreateDeviceRGB())
            using (var gradient = new CGGradient(cspace, components, locations))
            {

                var sPoint = new CGPoint(0, 0);
                var ePoint = new CGPoint(0, maxY);
                context.DrawLinearGradient(gradient, sPoint, ePoint, (CGGradientDrawingOptions)0);
            }
            context.RestoreState();
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        void DrawFrame(CGRect rect, CGContext context)
        {
            nfloat radius = rect.GetMaxY() * CornerRoundness;
            nfloat puffer = rect.GetMaxY() * 0.10f;
            nfloat maxX = rect.GetMaxX() - puffer;
            nfloat maxY = rect.GetMaxY() - puffer;
            nfloat minX = rect.GetMinX() + puffer;
            nfloat minY = rect.GetMinY() + puffer;
            context.BeginPath();
            nfloat lineSize = 2;
            if (ScaleFactor > 1)
            {
                lineSize += ScaleFactor * 0.25f;
            }
            context.SetLineWidth(lineSize);
            context.SetStrokeColor(FrameColor.CGColor);

            context.AddArc(maxX - radius, minY + radius, radius, M_PI + (M_PI / 2f), 0f, false);
            context.AddArc(maxX - radius, maxY - radius, radius, 0, M_PI / 2f, false);
            context.AddArc(minX + radius, maxY - radius, radius, M_PI / 2f, M_PI, false);
            context.AddArc(minX + radius, minY + radius, radius, M_PI, M_PI + (M_PI / 2f), false);
            context.ClosePath();
            context.StrokePath();
        }

        /// <summary>
        /// 不阻塞点击事件
        /// </summary>
        public override bool PointInside(CGPoint point, UIEvent uievent) => false;

        /// <summary>
        /// 只有使用 AutoLayout 布局时才会调用, AutoLayout 决定位置, 该值决定其大小
        /// </summary>
        public override CGSize IntrinsicContentSize => contentSize;
    }
}
