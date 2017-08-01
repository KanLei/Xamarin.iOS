using System;

using UIKit;
using Foundation;

using MvvmCross.iOS.Views;
using MvvmCross.Core.ViewModels;

using Masonry;
using MvvmCross.Binding.BindingContext;
using Core.ViewModels.Base;

namespace iOS.Views
{
    /// <summary>
    /// 支持自定义 NavigationBar，一旦启用，
    /// 后续应使用 RealView 代替 View
    /// </summary>
    public abstract class BaseView<TViewModel> : MvxViewController<TViewModel> where TViewModel : BaseViewModel
    {
        // 容器视图：NavigationBar + View
        private readonly UIView containerView = new UIView();

        private NSObject keyboardShow, keyboardHidden;

        protected readonly XNavigationBar NavigationBar = new XNavigationBar();

        /// <summary>
        /// 是否启用 NavigationBar
        /// </summary>
        protected virtual bool EnableNavigationBar { get; } = true;

        /// <summary>
        /// View 是否呈现在 NavigationBar 底部
        /// </summary>
        protected virtual bool UnderNavigationBar { get; } = false;

        /// <summary>
        /// 是否启用键盘显示和隐藏通知
        /// </summary>
        protected bool EnableKeyboardNotify { get; } = false;

        /// <summary>
        /// 注意：一旦启用自定义导航条，应使用 RealView 代替 View
        /// </summary>
        public UIView RealView
        {
            get { return EnableNavigationBar ? View.Subviews[0] : View; }
        }

        public BaseView()
        {
            Initialize();
        }

        public BaseView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public BaseView(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            Initialize();
        }

        private void Initialize()
        {
            AutomaticallyAdjustsScrollViewInsets = false;
            HidesBottomBarWhenPushed = true;
        }


        public sealed override void LoadView()
        {
            base.LoadView();

            if (EnableNavigationBar)
            {
                SetupNavigationView();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupNavigationBarBinding();
        }

        private void SetupNavigationView()
        {
            SetupNavigationContainer();
            SetupNavigationBar();
        }

        private void SetupNavigationContainer()
        {
            var originView = this.View;
            containerView.AddSubview(originView);
            containerView.Frame = originView.Frame;
            this.View = containerView;

            originView.TranslatesAutoresizingMaskIntoConstraints = false;

            if (UnderNavigationBar)
            {
                originView.MakeConstraints(maker =>
                {
                    maker.Edges.EqualTo(containerView);
                });
            }
            else
            {
                var insets = new UIEdgeInsets(XNavigationBar.HEIGHT, 0, 0, 0);
                originView.MakeConstraints(maker =>
                {
                    maker.Edges.EqualTo(containerView).Insets(insets);
                });
            }
        }

        private void SetupNavigationBar()
        {
            containerView.AddSubview(NavigationBar);

            NavigationBar.TranslatesAutoresizingMaskIntoConstraints = false;
            NavigationBar.MakeConstraints(maker =>
            {
                maker.Top.Left.Right.EqualTo(containerView);
                maker.Height.EqualTo(NSNumber.FromNFloat(XNavigationBar.HEIGHT));
            });
        }

        protected virtual void SetupNavigationBarBinding()
        {
            var set = this.CreateBindingSet<BaseView<TViewModel>, TViewModel>();
            // title
            set.Bind(NavigationBar).For(bar => bar.Title).To(vm => vm.Header.Title);
            // left
            set.Bind(NavigationBar.LeftFirstButton).For("Title").To(vm => vm.Header.LeftFirstButton.Title);
            set.Bind(NavigationBar.LeftFirstButton).To(vm => vm.Header.LeftFirstButton.TapCommand);
            set.Bind(NavigationBar.LeftSecondButton).For("Title").To(vm => vm.Header.LeftSecondButton.Title);
            set.Bind(NavigationBar.LeftSecondButton).To(vm => vm.Header.LeftSecondButton.TapCommand);
            // right
            set.Bind(NavigationBar.RightFirstButton).For("Title").To(vm => vm.Header.RightFirstButton.Title);
            set.Bind(NavigationBar.RightFirstButton).To(vm => vm.Header.RightFirstButton.TapCommand);
            set.Bind(NavigationBar.RightSecondButton).For("Title").To(vm => vm.Header.RightSecondButton.Title);
            set.Bind(NavigationBar.RightSecondButton).To(vm => vm.Header.RightSecondButton.TapCommand);
            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            keyboardShow = UIKeyboard.Notifications.ObserveWillShow(KeyboardWillShow);
            keyboardHidden = UIKeyboard.Notifications.ObserveWillHide(KeyboardWillHide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            keyboardShow?.Dispose();
            keyboardHidden?.Dispose();
        }

        protected virtual void KeyboardWillShow(object sender, UIKeyboardEventArgs e)
        {
            // 用于子类重写
        }

        protected virtual void KeyboardWillHide(object sender, UIKeyboardEventArgs e)
        {
            // 用于子类重写
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() => UIInterfaceOrientationMask.Portrait;
    }
}
