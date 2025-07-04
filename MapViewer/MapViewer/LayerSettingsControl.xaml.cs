using MapControl;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace MapViewer
{
	public sealed partial class LayerSettingsControl : UserControl
	{
		private readonly Map _map;
		private readonly Panel _layer;
		//private readonly MapImageLayer? _imageLayer;
		//private readonly MapTileLayerBase? _tileLayer;

		public LayerSettingsControl(MapImageLayer layer, Map map)
		{
			InitializeComponent();

			_layer = layer;
			_map = map;
		}

		public LayerSettingsControl(MapTileLayerBase layer, Map map)
		{
			InitializeComponent();

			_layer = layer;
			_map = map;
		}

		public required string LayerName { get; set; }

		public bool IsLayerEnabled
		{
			get => EnabledToggleSwitch.IsOn;
			set => EnabledToggleSwitch.IsOn = value;
		}

		public double LayerOpacity
		{
			get => OpacitySlider.Value;
			set => OpacitySlider.Value = Math.Clamp(value, 0.0, 100.0);
		}

		private void EnabledToggleSwitch_Toggled(object sender, RoutedEventArgs e)
		{
			if (EnabledToggleSwitch.IsOn)
			{
				if (!_map.Children.Contains(_layer))
				{
					_map.Children.Add(_layer);
				}
			}
			else
			{
				_map.Children.Remove(_layer);
			}
		}

		private void OpacitySlider_ValueChanged(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
		{
			if (_layer != null)
			{
				_layer.Opacity = e.NewValue / 100.0;
			}
		}
	}
}
