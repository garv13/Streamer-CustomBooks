﻿#pragma checksum "C:\Users\garvj\Source\Repos\Streamer-CustomBooks\Heist\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2AF90209827449BC3239043FD7189154"
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
    partial class Login : 
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
                    this.LoadingBar = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                }
                break;
            case 2:
                {
                    this.UserName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3:
                {
                    this.Password = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                    #line 42 "..\..\..\Login.xaml"
                    ((global::Windows.UI.Xaml.Controls.PasswordBox)this.Password).KeyDown += this.Password_KeyDown;
                    #line default
                }
                break;
            case 4:
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 43 "..\..\..\Login.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.Button_Click;
                    #line default
                }
                break;
            case 5:
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 44 "..\..\..\Login.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.Button_Click_1;
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

