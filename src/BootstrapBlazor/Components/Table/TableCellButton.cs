﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using System;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 单元格内按钮组件
    /// </summary>
    public class TableCellButton : ButtonBase, IDisposable
    {
        /// <summary>
        /// 获得/设置 Table Toolbar 实例
        /// </summary>
        [CascadingParameter]
        protected TableExtensionButton? Buttons { get; set; }

        /// <summary>
        /// 获得/设置 点击按钮是否选中正行 默认 true 选中
        /// </summary>
        [Parameter]
        public bool AutoSelectedRowWhenClick { get; set; } = true;

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Buttons?.AddButton(this);

            if (Size == Size.None)
            {
                Size = Size.ExtraSmall;
            }
        }

        /// <summary>
        /// Dispose 方法
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Buttons?.RemoveButton(this);
            }
        }

        /// <summary>
        /// Dispose 方法
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
