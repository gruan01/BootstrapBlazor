﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// Dialog 组件服务
    /// </summary>
    public class DialogService : BootstrapServiceBase<DialogOption>
    {
        private IStringLocalizer<EditDialog<DialogOption>> EditDialogLocalizer { get; set; }

        private IStringLocalizer<SearchDialog<DialogOption>> SearchDialogLocalizer { get; set; }

        private IStringLocalizer<ResultDialogOption> ResultDialogLocalizer { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="editLocalizer"></param>
        /// <param name="seachLocalizer"></param>
        /// <param name="resultDialogLocalizer"></param>
        public DialogService(
            IStringLocalizer<EditDialog<DialogOption>> editLocalizer,
            IStringLocalizer<SearchDialog<DialogOption>> seachLocalizer,
            IStringLocalizer<ResultDialogOption> resultDialogLocalizer)
        {
            EditDialogLocalizer = editLocalizer;
            SearchDialogLocalizer = seachLocalizer;
            ResultDialogLocalizer = resultDialogLocalizer;
        }

        /// <summary>
        /// 显示 Dialog 方法
        /// </summary>
        /// <param name="option">弹窗配置信息实体类</param>
        /// <param name="dialog">指定弹窗组件 默认为 null 使用 <see cref="BootstrapBlazorRoot"/> 组件内置弹窗组件</param>
        /// <returns></returns>
        public Task Show(DialogOption option, Dialog? dialog = null) => Invoke(option, dialog);

        /// <summary>
        /// 弹出搜索对话框
        /// </summary>
        /// <param name="option">SearchDialogOption 配置类实例</param>
        /// <param name="dialog">指定弹窗组件 默认为 null 使用 <see cref="BootstrapBlazorRoot"/> 组件内置弹窗组件</param>
        public async Task ShowSearchDialog<TModel>(SearchDialogOption<TModel> option, Dialog? dialog = null)
        {
            option.ResetButtonText ??= SearchDialogLocalizer[nameof(option.ResetButtonText)];
            option.QueryButtonText ??= SearchDialogLocalizer[nameof(option.QueryButtonText)];

            var parameters = new Dictionary<string, object>
            {
                [nameof(SearchDialog<TModel>.ShowLabel)] = option.ShowLabel,
                [nameof(SearchDialog<TModel>.Items)] = option.Items ?? Utility.GenerateColumns<TModel>(item => item.Searchable),
                [nameof(SearchDialog<TModel>.OnResetSearchClick)] = new Func<Task>(async () =>
                {
                    option.OnCloseAsync = null;
                    option.Dialog.RemoveDialog();
                    if (option.OnResetSearchClick != null)
                    {
                        await option.OnResetSearchClick();
                    }
                }),
                [nameof(SearchDialog<TModel>.OnSearchClick)] = new Func<Task>(async () =>
                {
                    option.OnCloseAsync = null;
                    option.Dialog.RemoveDialog();
                    if (option.OnSearchClick != null)
                    {
                        await option.OnSearchClick();
                    }
                }),
                [nameof(SearchDialog<TModel>.RowType)] = option.RowType,
                [nameof(SearchDialog<TModel>.LabelAlign)] = option.LabelAlign
            };

            if (option.ItemsPerRow.HasValue)
            {
                parameters.Add(nameof(ItemsPerRow), option.ItemsPerRow);
            }

            if (!string.IsNullOrEmpty(option.ResetButtonText))
            {
                parameters.Add(nameof(SearchDialog<TModel>.ResetButtonText), option.ResetButtonText);
            }

            if (!string.IsNullOrEmpty(option.QueryButtonText))
            {
                parameters.Add(nameof(SearchDialog<TModel>.QueryButtonText), option.QueryButtonText);
            }

            if (option.Model != null)
            {
                parameters.Add(nameof(SearchDialog<TModel>.Model), option.Model);
            }

            if (option.DialogBodyTemplate != null)
            {
                parameters.Add(nameof(SearchDialog<TModel>.BodyTemplate), option.DialogBodyTemplate);
            }

            option.Component = BootstrapDynamicComponent.CreateComponent<SearchDialog<TModel>>(parameters);

            await Invoke(option, dialog);
        }

        /// <summary>
        /// 弹出编辑对话框
        /// </summary>
        /// <param name="option">EditDialogOption 配置类实例</param>
        /// <param name="dialog"></param>
        public async Task ShowEditDialog<TModel>(EditDialogOption<TModel> option, Dialog? dialog = null)
        {
            option.CloseButtonText ??= EditDialogLocalizer[nameof(option.CloseButtonText)];
            option.SaveButtonText ??= EditDialogLocalizer[nameof(option.SaveButtonText)];

            var parameters = new Dictionary<string, object>
            {
                [nameof(EditDialog<TModel>.ShowLoading)] = option.ShowLoading,
                [nameof(EditDialog<TModel>.ShowLabel)] = option.ShowLabel,
                [nameof(EditDialog<TModel>.Items)] = option.Items ?? Utility.GenerateColumns<TModel>(item => item.Editable),
                [nameof(EditDialog<TModel>.OnCloseAsync)] = new Func<Task>(async () =>
                {
                    option.Dialog.RemoveDialog();
                    await option.Dialog.CloseOrPopDialog();
                }),
                [nameof(EditDialog<TModel>.OnSaveAsync)] = new Func<EditContext, Task>(async context =>
                {
                    if (option.OnSaveAsync != null)
                    {
                        var ret = await option.OnSaveAsync(context);
                        if (ret)
                        {
                            option.Dialog.RemoveDialog();
                            await option.Dialog.CloseOrPopDialog();
                        }
                    }
                }),
                [nameof(EditDialog<TModel>.RowType)] = option.RowType,
                [nameof(EditDialog<TModel>.LabelAlign)] = option.LabelAlign,
                [nameof(EditDialog<TModel>.IsTracking)] = option.IsTracking,
                [nameof(EditDialog<TModel>.ItemChangedType)] = option.ItemChangedType
            };

            if (option.ItemsPerRow.HasValue)
            {
                parameters.Add(nameof(ItemsPerRow), option.ItemsPerRow);
            }

            if (!string.IsNullOrEmpty(option.CloseButtonText))
            {
                parameters.Add(nameof(EditDialog<TModel>.CloseButtonText), option.CloseButtonText);
            }

            if (!string.IsNullOrEmpty(option.SaveButtonText))
            {
                parameters.Add(nameof(EditDialog<TModel>.SaveButtonText), option.SaveButtonText);
            }

            if (option.Model != null)
            {
                parameters.Add(nameof(EditDialog<TModel>.Model), option.Model);
            }

            if (option.DialogBodyTemplate != null)
            {
                parameters.Add(nameof(EditDialog<TModel>.BodyTemplate), option.DialogBodyTemplate);
            }

            option.Component = BootstrapDynamicComponent.CreateComponent<EditDialog<TModel>>(parameters);

            await Invoke(option, dialog);
        }

        /// <summary>
        /// 弹出带结果的对话框
        /// </summary>
        /// <param name="option">对话框参数</param>
        /// <param name="dialog">指定弹窗组件 默认为 null 使用 <see cref="BootstrapBlazorRoot"/> 组件内置弹窗组件</param>
        /// <returns></returns>
        public async Task<DialogResult> ShowModal<TDialog>(ResultDialogOption option, Dialog? dialog = null)
            where TDialog : IComponent, IResultDialog
        {
            IResultDialog? resultDialog = null;
            var result = DialogResult.Close;

            option.BodyTemplate = builder =>
            {
                builder.OpenComponent(0, typeof(TDialog));
                builder.AddMultipleAttributes(1, option.ComponentParamters);
                builder.AddComponentReferenceCapture(2, com => resultDialog = (IResultDialog)com);
                builder.CloseComponent();
            };

            option.FooterTemplate = BootstrapDynamicComponent.CreateComponent<ResultDialogFooter>(new Dictionary<string, object>
            {
                [nameof(ResultDialogFooter.ShowCloseButton)] = option.ShowCloseButton,
                [nameof(ResultDialogFooter.ButtonCloseColor)] = option.ButtonCloseColor,
                [nameof(ResultDialogFooter.ButtonCloseIcon)] = option.ButtonCloseIcon,
                [nameof(ResultDialogFooter.ButtonCloseText)] = option.ButtonCloseText ?? ResultDialogLocalizer[nameof(option.ButtonCloseText)].Value,
                [nameof(ResultDialogFooter.OnClickClose)] = new Func<Task>(async () =>
                {
                    result = DialogResult.Close;
                    if (option.OnCloseAsync != null) { await option.OnCloseAsync(); }
                }),

                [nameof(ResultDialogFooter.ShowYesButton)] = option.ShowYesButton,
                [nameof(ResultDialogFooter.ButtonYesColor)] = option.ButtonYesColor,
                [nameof(ResultDialogFooter.ButtonYesIcon)] = option.ButtonYesIcon,
                [nameof(ResultDialogFooter.ButtonYesText)] = option.ButtonYesText ?? ResultDialogLocalizer[nameof(option.ButtonYesText)].Value,
                [nameof(ResultDialogFooter.OnClickYes)] = new Func<Task>(async () =>
                {
                    result = DialogResult.Yes;
                    if (option.OnCloseAsync != null) { await option.OnCloseAsync(); }
                }),

                [nameof(ResultDialogFooter.ShowNoButton)] = option.ShowNoButton,
                [nameof(ResultDialogFooter.ButtonNoColor)] = option.ButtonNoColor,
                [nameof(ResultDialogFooter.ButtonNoIcon)] = option.ButtonNoIcon,
                [nameof(ResultDialogFooter.ButtonNoText)] = option.ButtonNoText ?? ResultDialogLocalizer[nameof(option.ButtonNoText)].Value,
                [nameof(ResultDialogFooter.OnClickNo)] = new Func<Task>(async () =>
                {
                    result = DialogResult.No;
                    if (option.OnCloseAsync != null) { await option.OnCloseAsync(); }
                })
            }).Render();

            var closeCallback = option.OnCloseAsync;
            option.OnCloseAsync = async () =>
            {
                if (resultDialog != null && await resultDialog.OnClosing(result))
                {
                    await resultDialog.OnClose(result);
                    if (closeCallback != null)
                    {
                        await closeCallback();
                    }

                    // Modal 与 ModalDialog 的 OnClose 事件陷入死循环
                    // option.OnClose -> Modal.Close -> ModalDialog.Close -> ModalDialog.OnClose -> option.OnClose
                    option.OnCloseAsync = null;
                    await option.Dialog.Close();
                    option.ReturnTask.SetResult(result);
                }
            };

            await Invoke(option, dialog);
            return await option.ReturnTask.Task;
        }
    }
}
