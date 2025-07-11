# Firelink

Firelink is a modern .NET 8 project that synchronizes the color and effects of your WLED-enabled LED device (running on ESP32) with the album art of the music you're listening to on Spotify. The project features a server-side rendered (SSR) Blazor web UI and leverages your custom `nweld` library for robust WLED integration. Firelink runs locally on your machine for privacy and performance.

## Project Architecture

Firelink is organized into three main layers:

1. **Presentation Layer:**  
   A server-side rendered (SSR) Blazor application providing a responsive and interactive web interface for controlling and monitoring your WLED device.

2. **Application Layer:**  
   Implements the CQRS pattern using MediatR to handle business logic and coordinate interactions between the Presentation and Infrastructure layers.

3. **Infrastructure Layer:**  
   Handles communication with external services, including the Spotify API for album data and your WLED device via the custom `nweld` library.

## Features

- **Spotify Album Color Sync:**  
  Automatically synchronizes your WLED device’s color with the dominant color of the current Spotify album art.

- **Webscraping for Color Extraction:**  
  Firelink scrapes the Spotify web page to determine the dominant album color when needed.

- **Color Caching:**  
  Previously known album colors are cached and stored in a file for fast future lookups, reducing the need for repeated webscraping.

- **WLED Integration:**  
  Direct control of WLED devices running on ESP32 boards, using the custom `nweld` library for robust and efficient communication.

- **Custom Effects for Tracks and Albums:**  
  Users can define custom WLED effects or presets for specific Spotify tracks or albums. When a matching track or album is played, Firelink applies the user-defined effect or preset to the LEDs. A default effect can also be configured.

- **Color Conversion:**  
  Converts album art RGB colors to HSV for accurate color representation on your LEDs.

- **Local, Private, and Fast:**  
  Firelink runs entirely on your own machine—no cloud, no external servers, and no SEO concerns.

- **Settings and Customization:**  
  Easily manage device settings, custom effects, and more through the web interface.

- **Persistent User Data:**  
  User data and settings are saved locally to files using efficient serialization.

## Technology Stack

- **.NET 8**  
- **Blazor (Server-Side/SSR)**  
- **MediatR** (CQRS pattern)  
- **SpotifyAPI.Web** (Spotify integration)  
- **HtmlAgilityPack** (webscraping Spotify for album color)  
- **nweld** (custom WLED control library, developed in-house)  
- **Serilog** (logging)  
- **LazyCache** (caching)  
- **Polly** (resilience and transient-fault handling)  
- **MemoryPack** (efficient serialization for caching album colors and user data)  
- **OneOf** (discriminated unions for result handling)

## Installation

1. Download the latest release of Firelink for your platform.
2. Before running the app, open the `appsettings.json` file and update it with your own Spotify credentials and the URL of your WLED device.
3. Run the application—no further setup required!

## Usage

- Use the web interface to control your WLED device, view current track info, and manage custom effects.
- Make sure your Spotify credentials and WLED URL are correctly set in `appsettings.json` before starting the app.

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss your ideas.

## License

_TODO: Add license details._

