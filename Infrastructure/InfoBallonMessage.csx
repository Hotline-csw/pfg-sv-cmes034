#r ControllerMES.Infrastructure.Common
#r HomagGroup.ResourceHelper
#r ControllerMES.Infrastructure.Resource
#r PresentationFramework
#r PresentationCore
#r WindowsBase

//-----------------------------------------------------------------------------
//   (Class-)Name:   InfoBallonMessage
//
//   Description:    
//
//
//
//   Requirements:   <eg. DB-Tables/Attributes, ...>
//
//   Author:         Benjamin Schmidt
//   Date:           2020-04-03
//
//-----------------------------------------------------------------------------
//   Revision History:
//   Name            Date          Description
//   <Author>        2020-04-03    Created
//   
//-----------------------------------------------------------------------------


using System.Windows;
using System.Windows.Controls;
using HomagGroup.Base.UI;
using HomagGroup.Base.UI.Controls;
using System;
using System.Collections;
using System.Globalization;
using System.ComponentModel.Composition;

[Export(typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Export("InfoBallonMessage", typeof(HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization))]
[Description("Meldungsausgabe via InfoBallon")]
[EnabledScript(true)]
public class InfoBallonMessage : UserExitBase, HomagGroup.FLS.Infrastructure.Common.Customization.IClientCustomization
{
    public void UserFeedback(string title, string message, int timeToClose,  HomagGroup.Base.UI.DeviceState state)
    {
        //Parameter für Anzeige
        const int height = 300;
        const int width = 600;
        const int fontSizeText = 24;

        System.Windows.Application.Current.Dispatcher.Invoke(delegate
        {
            InfoBalloon infoBallon = new InfoBalloon();
            HomagGroup.Base.UI.UIAdjustments.SetScaleWindow(infoBallon, true);
            infoBallon.PlacementTarget = null;
            infoBallon.Placement = InfoBalloonPlacement.Top;
            infoBallon.DeviceState = state;
            infoBallon.TimeToClose = timeToClose;
            infoBallon.Label = title;
            infoBallon.Height = height;
            infoBallon.Width = width;
            infoBallon.Topmost = true;
            infoBallon.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            infoBallon.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;


            var wrapPanel = new WrapPanel();
            var textBox = new TextBlock();

            textBox.Text = message;
            textBox.MaxHeight = height;
            textBox.FontSize = fontSizeText;
            //textBox.MaxLength = width;
            //textBox.IsReadOnly = true;
            //textBox.BorderBrush = Brushes.White;
            textBox.FontStyle = default(System.Windows.FontStyle);

            wrapPanel.Children.Add(textBox);

            infoBallon.Content = wrapPanel;
            infoBallon.IsOpen = true;
        });
    }

    public void UserFeedbackDetail(string title, string message, List<string> collection, int timeToClose,  HomagGroup.Base.UI.DeviceState state)
    {
        //Parameter für Anzeige
        const int height = 600;
        const int width = 600;
        const int fontSizeText = 24;

        System.Windows.Application.Current.Dispatcher.Invoke(delegate
        {
            InfoBalloon infoBallon = new InfoBalloon();
            HomagGroup.Base.UI.UIAdjustments.SetScaleWindow(infoBallon, true);
            infoBallon.PlacementTarget = null;
            infoBallon.Placement = InfoBalloonPlacement.Top;
            infoBallon.DeviceState = state;
            infoBallon.TimeToClose = timeToClose;
            infoBallon.Label = title;
            infoBallon.Height = height;
            infoBallon.Width = width;
            infoBallon.Topmost = true;

            //Create my Grid
            var grid = new Grid();

            //Rows
            RowDefinition rowDef1 = new RowDefinition();
            //rowDef1.Height = new GridLength(300);
            RowDefinition rowDef2 = new RowDefinition();
            //rowDef2.Height = new GridLength(60);
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);

            //Erste Zeile des Grids ertsellen-- > Text anzeige der Message
            var textBlock = new TextBlock();
            textBlock.Text = message;
            //textBlock.MaxHeight = 200;
            textBlock.FontSize = fontSizeText;
            //textBox.MaxLength = width;
            textBlock.FontStyle = default(System.Windows.FontStyle);
            textBlock.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(textBlock, 0);
            grid.Children.Add(textBlock);

            //Zweite Zeile
            var textRows = "";
            foreach (var row in collection)
            {
                textRows += row + "\n";
            }
            var textBox = new System.Windows.Controls.TextBox();
            textBox.Text = textRows;
            textBox.MaxHeight = height;
            textBox.FontSize = fontSizeText;
            textBox.MaxLength = width;
            textBox.IsReadOnly = true;
            textBox.BorderBrush = System.Windows.Media.Brushes.White;
            textBox.FontStyle = default(System.Windows.FontStyle);
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            Grid.SetRow(textBox, 1);
            grid.Children.Add(textBox);

            infoBallon.Content = grid;
            infoBallon.IsOpen = true;
        });
    }
}
