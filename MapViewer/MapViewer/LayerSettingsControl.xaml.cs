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

			map.Children.Add(layer);

			_layer = layer;
			_map = map;
		}

		public LayerSettingsControl(MapTileLayerBase layer, Map map)
		{
			InitializeComponent();

			map.Children.Add(layer);

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
			get => OpacitySlider.Value / 100.0;
			set => OpacitySlider.Value = Math.Clamp(value * 100.0, 0.0, 100.0);
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
				_map.Children.Remove(_map);
			}
		}
	}
}
