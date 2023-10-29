import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

import 'testDeviceConnection.dart';
import 'deviceScheduleScreen.dart';
import '../appFrame.dart';

class Device {
  final int id;
  final String name;
  final String type;
  final String status;

  Device(this.id, this.name, this.type, this.status);
}

class DeviceConnectionsScreen extends StatefulWidget {
  const DeviceConnectionsScreen({super.key});

  @override
  State<DeviceConnectionsScreen> createState() =>
      _DeviceConnectionsScreenState();
}

class _DeviceConnectionsScreenState extends State<DeviceConnectionsScreen> {
  List<Device> devices = [];

  @override
  void initState() {
    super.initState();
    fetchDevices();
  }

  Future<void> fetchDevices() async {
    final response = await http.get(Uri.parse(
        'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/Devices'));

    if (response.statusCode == 200) {
      final List<dynamic> data = json.decode(response.body);
      print(data);
      List<Device> storedDevices = data
      .where((storedDevices) => storedDevices['userId'],)
      .map((storedDevices){
        return Device(
          storedDevices['id'],
          storedDevices['name'],
          storedDevices['type'],
          storedDevices['status']
        );
      }).toList();
      //TODO: Filter and map the devices from the API Server.
      setState(() {
        devices = storedDevices;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Device Management"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      body: ListView.builder(
        itemCount: devices.length,
        itemBuilder: (context, index) {
          final device = devices[index];
          return ListTile(
            title: Text(device.name),
            subtitle: Text('Type: ${device.type}'),
            leading: _buildIconForType(device.type),
            trailing: _buildStatusWidget(device),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => DeviceSchedulePage(device: device),
                ),
              );
            },
          );
        },
      ),
      bottomNavigationBar: Padding(
        padding: const EdgeInsets.all(16.0),
        child: ElevatedButton(
          onPressed: () {
            Navigator.push(
              context,
              MaterialPageRoute(
                  builder: (context) => const TestDeviceConnection()),
            );
          },
          child: const Text('Connect Devices'),
        ),
      ),
    );
  }

  Widget _buildIconForType(String type) {
    IconData iconData;

    switch (type) {
      case 'alarm':
        iconData = Icons.alarm;
        break;
      case 'light':
        iconData = Icons.lightbulb;
        break;
      case 'thermostat':
        iconData = Icons.thermostat;
        break;
      default:
        iconData = Icons.device_unknown;
    }

    return Icon(iconData);
  }

  Widget _buildStatusWidget(Device device) {
    if (device.type == 'alarm') {
      return Text('Next Alarm: ${device.status}');
    } else if (device.type == 'light') {
      return Text('Light: ${device.status}%');
    } else if (device.type == 'thermostat') {
      return Text('Temperature: ${device.status}°F');
    }
    return Text('Status: ${device.status}');
  }
}
