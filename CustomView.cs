using UIKit;


// Custom Control with Autolayout: https://www.objc.io/issues/3-views/advanced-auto-layout-toolbox/
// Alignment Rect: http://www.informit.com/articles/article.aspx?p=2041295&seqNum=9

namespace DemoApp
{
    public class CustomView:UIView
    {
        public CustomView()
        {
        }

        // 计算过程
        // Down-Top 从子视图到父视图的过程
        // 任何对约束本身的更改，都会自动触发该过程
        // 显示地触发该过程通过调用 SetNeedsUpdateConstraints()
        public override void UpdateConstraints()
        {
            // 为 subview 增加自定义约束
            // 避免重复添加约束

            base.UpdateConstraints();  // must call base
        }


        // 将 UpdateConstraints 的解决方案应用到视图的 center 和 bound
        // Top-Down 从父视图到子视图的过程
        // 显示地触发该过程通过调用 SetNeedsLayout()
        // 立即触发该过程通过调用 LayoutIfNeed()
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            // 在 View 布局之后，根据需要重新定位

            // 如果使用的控件要支持多行文本，则在这里设置 referredMaxLayoutWidth 用于计算固有大小，并重新调用 base.LayoutSubviews();
            // 或者在控件的子类的 LayoutSubViews 方法中 this.PreferredMaxLayoutWidth = this.Frame.Size.Width;base.LayoutSubviews();
            // 此时无需先调用 base，因为执行子类的 LayoutSubviews 方法时，Frame 已经确定，LayoutSubviews 自顶向下布局
        }


        // 显示过程
        // Top-Down 从父视图到子视图的过程
        // 不依赖于是否采用 AutoLayout
        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);


        }


        // 固有尺寸，自动转化为约束
        // 任何对固有尺寸有影响的改变都应该调用 InvalidateIntrinsicContentSize();
        public override CoreGraphics.CGSize IntrinsicContentSize
        {
            get
            {
                return base.IntrinsicContentSize;
            }
        }

        #region 以下两个方法不能在子类中重写，而是在实例化 View 时，显式地指定
        // 压缩阻力，IntrinsicContentSize 存在的条件下起作用
        public override float ContentCompressionResistancePriority(UILayoutConstraintAxis axis)
        {
            return base.ContentCompressionResistancePriority(axis);
        }


        // 吸附阻力，IntrinsicContentSize 存在的条件下起作用
        public override float ContentHuggingPriority(UILayoutConstraintAxis axis)
        {
            return base.ContentHuggingPriority(axis);
        }
        #endregion

        // 对齐矩形
        // Auto Layout does not operate on views’ frame, but on their alignment rect.
        // https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIView_Class/#//apple_ref/occ/instm/UIView/alignmentRectInsets
        public override UIEdgeInsets AlignmentRectInsets
        {
            get
            {
                return base.AlignmentRectInsets;
            }
        }


        public override UIView ViewForBaselineLayout
        {
            get
            {
                return base.ViewForBaselineLayout;
            }
        }

    }
}

