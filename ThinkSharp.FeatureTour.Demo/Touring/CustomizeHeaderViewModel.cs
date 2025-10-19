// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ThinkSharp.FeatureTouring.Touring
{
    public class CustomizeHeaderViewModel : ObservableObject
    {
        private string _header = "My Header";

        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }
    }
}
