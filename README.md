# Firelink

Firelink is a .NET 8 project that seamlessly changes the color of your Tuya LED device to match the album color you're listening to on Spotify. It features a responsive web UI built with Blazor WebAssembly and leverages SignalR for real-time communication. The project follows the CQRS pattern using MediatR on the backend and communicates with the Tuya Cloud API to manage the LED device.

## Project Architecture

Firelink is composed of three distinct layers:

1. **Presentation Layer:** This layer includes the user interface of the application, designed with Blazor WebAssembly, and uses SignalR for real-time updates. (Client, Server, Shared)

2. **Application Layer:** The heart of Firelink, implementing the CQRS pattern with MediatR for handling business logic and interactions between the Presentation and Infrastructure layers. (Application)

3. **Infrastructure Layer:** This layer is responsible for communications with external systems and services, such as the Tuya Cloud API and Spotify API. (Infrastructure, TuyaConnector)

## Features

- **Spotify Album Color Sync:** Synchronizes the color of your Tuya LED device with the predominant color of the album you're currently listening to on Spotify.
- **Color Conversion:** Automatically converts the RGB color from the album cover to HSV before sending commands to the Tuya Cloud API.
- **Real-time Updates:** Utilizes SignalR to provide real-time updates on the web interface.

## Installation

_TODO: Add installation instructions_

## Usage

_TODO: Add usage instructions_

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

_TODO: Add license details_

