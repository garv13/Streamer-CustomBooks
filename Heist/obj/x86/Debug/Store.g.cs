﻿#pragma checksum "C:\Users\garvj\Source\Repos\Streamer-CustomBooks\Heist\Store.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7001A8C704771EFA76DCD1C573B35171"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Heist
{
    partial class Store : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.MySplitView = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 2:
                {
                    this.HamburgerButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 68 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.HamburgerButton).Click += this.HamburgerButton_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.MenuButton2 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 71 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.MenuButton2).Click += this.MenuButton2_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.MenuButton3 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 74 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.MenuButton3).Click += this.MenuButton3_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.MenuButton7 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 77 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.MenuButton7).Click += this.MenuButton7_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.MenuButton4 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 80 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.MenuButton4).Click += this.MenuButton4_Click;
                    #line default
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.RadioButton element7 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 83 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)element7).Click += this.MenuButton1_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.MenuButton5 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 86 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.MenuButton5).Click += this.MenuButton5_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.MenuButton6 = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    #line 89 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.MenuButton6).Click += this.MenuButton6_Click;
                    #line default
                }
                break;
            case 10:
                {
                    this.LoadingBar2 = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                }
                break;
            case 11:
                {
                    this.Box2 = (global::Syncfusion.UI.Xaml.Controls.Input.SfTextBoxExt)(target);
                }
                break;
            case 12:
                {
                    this.SearchButton2 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 167 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SearchButton2).Click += this.SearchButton2_Click;
                    #line default
                }
                break;
            case 13:
                {
                    this.StoreListView2 = (global::Windows.UI.Xaml.Controls.GridView)(target);
                    #line 181 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.StoreListView2).ItemClick += this.StoreListView2_ItemClick;
                    #line default
                }
                break;
            case 14:
                {
                    this.LoadingBar = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                }
                break;
            case 15:
                {
                    this.Box = (global::Syncfusion.UI.Xaml.Controls.Input.SfTextBoxExt)(target);
                }
                break;
            case 16:
                {
                    this.SearchButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 119 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SearchButton).Click += this.Button_Click;
                    #line default
                }
                break;
            case 17:
                {
                    this.StoreListView = (global::Windows.UI.Xaml.Controls.GridView)(target);
                    #line 132 "..\..\..\Store.xaml"
                    ((global::Windows.UI.Xaml.Controls.GridView)this.StoreListView).ItemClick += this.StoreListView_ItemClick;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

