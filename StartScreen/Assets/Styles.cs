using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StartScreen.Assets
{
    internal class Styles
    {
        public static ControlTemplate circleButtonTemplate = circleButtonTemplateSetup();
        public static Style circleButtonStyle = circleButtonStyleSetup();

        public static Style LargeTileStyle = largeTileStyleSetup(); // 2x2
        public static Style SmallTileStyle = smallTileStyleSetup(); // 1x1
        public static Style SmallerTileStyle = smallerTileStyleSetup(); // 0.5x0.5
        public static Style WideTileStyle = wideTileStyleSetup(); // 1x2

        public const double NormalTile = 125.0;

        public static ControlTemplate circleButtonTemplateSetup()
        {
            var result = new ControlTemplate(typeof(Button));
            var gridView = new FrameworkElementFactory(typeof(Grid));
            
            var ellipseItem = new FrameworkElementFactory(typeof(Ellipse));
            ellipseItem.SetValue(Ellipse.FillProperty, new SolidColorBrush(Colors.Transparent));
            ellipseItem.SetValue(Ellipse.StrokeProperty, new SolidColorBrush(Colors.White));
            ellipseItem.SetValue(Ellipse.StrokeThicknessProperty, 4.0);

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            gridView.AppendChild(ellipseItem);
            gridView.AppendChild(contentPresenter);
            result.VisualTree = gridView;

            return result;
        }

        public static Style circleButtonStyleSetup()
        {
            var result = new Style(typeof(Button));

            result.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            result.Setters.Add(new Setter(Button.ForegroundProperty, new SolidColorBrush(Colors.White)));
            result.Setters.Add(new Setter(Button.TemplateProperty, circleButtonTemplate));
            result.Setters.Add(new Setter(Button.FontFamilyProperty, new FontFamily("Segoe MDL2 Assets")));
            result.Setters.Add(new Setter(Button.FontSizeProperty, 12.0));
            result.Setters.Add(new Setter(Button.WidthProperty, 36.0));
            result.Setters.Add(new Setter(Button.HeightProperty, 36.0));

            var mouseEvtTrigger = new Trigger()
            {
                Property = Button.IsMouseOverProperty,
                Value = true
            };
            mouseEvtTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent)));
            mouseEvtTrigger.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.DimGray)));
            result.Triggers.Add(mouseEvtTrigger);

            return result;
        }

        public static Style largeTileStyleSetup()
        {
            var result = new Style(typeof(Tile));
            
            result.Setters.Add(new Setter(Tile.HeightProperty, NormalTile * 2));
            result.Setters.Add(new Setter(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display));
            result.Setters.Add(new Setter(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType));
            result.Setters.Add(new Setter(Tile.TitleFontSizeProperty, 14.0));
            result.Setters.Add(new Setter(Tile.WidthProperty, NormalTile * 2));
            result.Setters.Add(new Setter(Tile.ForegroundProperty, new SolidColorBrush(Colors.White)));

            var mouseEvtTrigger = new Trigger()
            {
                Property = Button.IsMouseOverProperty,
                Value = true
            };
            mouseEvtTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent)));
            mouseEvtTrigger.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            result.Triggers.Add(mouseEvtTrigger);

            return result;
        }

        public static Style smallTileStyleSetup()
        {
            var result = new Style(typeof(Tile));
            
            result.Setters.Add(new Setter(Tile.HeightProperty, NormalTile));
            result.Setters.Add(new Setter(TextOptions.TextFormattingModeProperty, TextFormattingMode.Ideal));
            result.Setters.Add(new Setter(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType));
            result.Setters.Add(new Setter(Tile.TitleFontSizeProperty, 10.0));
            result.Setters.Add(new Setter(Tile.WidthProperty, NormalTile));
            result.Setters.Add(new Setter(Tile.ForegroundProperty, new SolidColorBrush(Colors.White)));
            //result.Setters.Add(new Setter(Tile.PaddingProperty, new Padding))

            var mouseEvtTrigger = new Trigger()
            {
                Property = Button.IsMouseOverProperty,
                Value = true
            };
            mouseEvtTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent)));
            mouseEvtTrigger.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            result.Triggers.Add(mouseEvtTrigger);

            return result;
        }

        public static Style smallerTileStyleSetup()
        {
            var result = new Style(typeof(Tile));

            result.Setters.Add(new Setter(Tile.HeightProperty, NormalTile / 2));
            result.Setters.Add(new Setter(TextOptions.TextFormattingModeProperty, TextFormattingMode.Ideal));
            result.Setters.Add(new Setter(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType));
            result.Setters.Add(new Setter(Tile.TitleFontSizeProperty, 7.0));
            result.Setters.Add(new Setter(Tile.WidthProperty, NormalTile / 2));
            result.Setters.Add(new Setter(Tile.ForegroundProperty, new SolidColorBrush(Colors.White)));

            var mouseEvtTrigger = new Trigger()
            {
                Property = Button.IsMouseOverProperty,
                Value = true
            };
            mouseEvtTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent)));
            mouseEvtTrigger.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            result.Triggers.Add(mouseEvtTrigger);

            return result;
        }

        public static Style wideTileStyleSetup()
        {
            var result = new Style(typeof(Tile));

            result.Setters.Add(new Setter(Tile.HeightProperty, NormalTile * 2));
            result.Setters.Add(new Setter(TextOptions.TextFormattingModeProperty, TextFormattingMode.Ideal));
            result.Setters.Add(new Setter(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType));
            result.Setters.Add(new Setter(Tile.TitleFontSizeProperty, 10.0));
            result.Setters.Add(new Setter(Tile.WidthProperty, NormalTile * 2));
            result.Setters.Add(new Setter(Tile.ForegroundProperty, new SolidColorBrush(Colors.White)));

            var mouseEvtTrigger = new Trigger()
            {
                Property = Button.IsMouseOverProperty,
                Value = true
            };
            mouseEvtTrigger.Setters.Add(new Setter(Button.BorderBrushProperty, new SolidColorBrush(Colors.Transparent)));
            mouseEvtTrigger.Setters.Add(new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            result.Triggers.Add(mouseEvtTrigger);

            return result;
        }
    }
}
