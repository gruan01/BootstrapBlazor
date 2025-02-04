﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Components;
using BootstrapBlazor.Shared;
using Bunit;
using System;
using System.Threading.Tasks;
using UnitTest.Core;
using UnitTest.Extensions;
using Xunit;

namespace UnitTest.Components
{
    public class InputTest : BootstrapBlazorTestBase
    {
        [Fact]
        public void IsGroup_Ok()
        {
            // 显示标签时 IsGroup 属性才生效
            // 设置 IsGroup 为 true 
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.ShowLabel, true);
                builder.Add(a => a.IsGroup, true);
            });
            var span = cut.Find(".input-group-text");
            Assert.NotNull(span);

            // 设置 IsGroup 为 true 
            cut.SetParametersAndRender(builder => builder.Add(a => a.IsGroup, false));
            var label = cut.Find(".form-label");
            Assert.NotNull(label);

            // 设置不显示标签时 IsGroup 属性不生效
            cut.SetParametersAndRender(builder =>
            {
                builder.Add(a => a.ShowLabel, false);
                builder.Add(a => a.IsGroup, true);
            });
            var input = cut.Find(".form-control");
            Assert.NotNull(input);
        }

        [Fact]
        public void Color_Ok()
        {
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.Color, Color.None);
                builder.Add(a => a.IsDisabled, false);
            });
            Assert.DoesNotContain("border-", cut.Markup);

            cut.SetParametersAndRender(builder => builder.Add(a => a.Color, Color.Primary));
            Assert.Contains("border-primary", cut.Markup);
        }

        [Fact]
        public void PlaceHolder_Ok()
        {
            var ph = "placeholder_test";
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder => builder.Add(a => a.PlaceHolder, ph));
            Assert.Contains($"placeholder=\"{ph}\"", cut.Markup);
        }

        [Fact]
        public void Value_Ok()
        {
            var model = new Foo() { Name = "Test" };
            var valueChanged = false;
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.Value, model.Name);
                builder.Add(a => a.ValueChanged, v => model.Name = v);
                builder.Add(a => a.ValueExpression, model.GenerateValueExpression());
                builder.Add(a => a.OnValueChanged, v => { valueChanged = true; return Task.CompletedTask; });
            });
            cut.Find("input").Change("Test1");
            Assert.Contains(model.Name, "Test1");
            Assert.True(valueChanged);
        }

        [Fact]
        public void IsAutoFocus_Ok()
        {
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder => builder.Add(a => a.IsAutoFocus, true));
        }

        [Fact]
        public void Type_Ok()
        {
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder => builder.AddUnmatched("type", "number"));
            Assert.Contains($"type=\"number\"", cut.Markup);

            cut = Context.RenderComponent<BootstrapInput<string>>();
            Assert.Contains($"type=\"text\"", cut.Markup);
        }

        [Fact]
        public void IsTrim_Ok()
        {
            var val = "    test    ";
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.IsTrim, true);
                builder.Add(a => a.Value, "");
            });
            Assert.Equal("", cut.Instance.Value);
            cut.Find("input").Change(val);
            Assert.Equal(val.Trim(), cut.Instance.Value);

            cut.SetParametersAndRender(builder =>
            {
                builder.Add(a => a.IsTrim, false);
                builder.Add(a => a.Value, "");
            });
            Assert.Equal("", cut.Instance.Value);
            cut.Find("input").Change(val);
            Assert.Equal(val, cut.Instance.Value);
        }

        [Fact]
        public void Formatter_Ok()
        {
            var cut = Context.RenderComponent<BootstrapInput<DateTime>>(builder =>
            {
                builder.Add(a => a.FormatString, "yyyy-MM-dd");
                builder.Add(a => a.Value, DateTime.Now);
            });
            Assert.Contains($"value=\"{DateTime.Now:yyyy-MM-dd}\"", cut.Markup);

            cut.SetParametersAndRender(builder =>
            {
                builder.Add(a => a.FormatString, null);
                builder.Add(a => a.Formatter, dt => dt.ToString("HH:mm:ss"));
                builder.Add(a => a.Value, DateTime.Now);
            });
            Assert.Contains($"value=\"{DateTime.Now:HH:mm:ss}\"", cut.Markup);
        }

        [Fact]
        public async Task EnterCallback_Ok()
        {
            var val = "";
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.OnEnterAsync, v => { val = v; return Task.CompletedTask; });
                builder.Add(a => a.Value, "test");
            });
            await cut.Instance.EnterCallback();
            Assert.Equal("test", val);
        }

        [Fact]
        public async Task EscCallback_Ok()
        {
            var val = "";
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.OnEscAsync, v => { val = v; return Task.CompletedTask; });
            });
            await cut.Instance.EscCallback("test");
            Assert.Equal("test", val);
        }

        [Fact]
        public void IsSelectAllTextOnFocus_Ok()
        {
            var cut = Context.RenderComponent<BootstrapInput<string>>(builder =>
            {
                builder.Add(a => a.IsSelectAllTextOnFocus, true);
            });
        }
    }
}
