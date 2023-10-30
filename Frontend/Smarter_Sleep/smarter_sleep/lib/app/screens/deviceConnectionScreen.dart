import 'package:amplify_flutter/amplify_flutter.dart';
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
  final String? status;

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
      final user = await Amplify.Auth.getCurrentUser();
      final userId = user.userId;

      final List<dynamic> data = json.decode(response.body);
      List<Device> fetchedDevices = data
          .where((deviceData) => deviceData['userId'] == userId)
          .map((deviceData) {
        return Device(deviceData['id'], deviceData['name'], deviceData['type'],
            deviceData['status']);
      }).toList();
      setState(() {
        devices = fetchedDevices;
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
      body: devices.isEmpty
          ? const Center(
              child: Text('No devices connected, connect one below.'),
            )
          : ListView.builder(
              itemCount: devices.length,
              itemBuilder: (context, index) {
                final device = devices[index];
                return ListTile(
                  title: Text(device.name),
                  subtitle: Text('Type: ${device.type}'),
                  leading: _buildIconForType(device.type),
                  trailing: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      _buildStatusWidget(device),
                      IconButton(
                        icon: const Icon(Icons.edit),
                        onPressed: () {
                          _navigateToEditDevice(context, device);
                        },
                      ),
                    ],
                  ),
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) =>
                            DeviceSchedulePage(device: device),
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
            _navigateToAddDevice(context);
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
    if (device.status == null) {
      return const Text('No Status');
    }
    if (device.type == 'alarm') {
      final nextAlarm = DateTime.tryParse(device.status!);
      if (nextAlarm != null) {
        final now = DateTime.now();
        final timeDifference = nextAlarm.difference(now);

        if (timeDifference.inDays > 1) {
          final days = timeDifference.inDays;
          return Text('Next Alarm: in $days days');
        } else {
          final formattedTime = "${nextAlarm.hour}:${nextAlarm.minute}";
          return Text('Next Alarm: $formattedTime');
        }
      }
      return Text('Next Alarm\n ${device.status}');
    } else if (device.type == 'light') {
      return Text('Light: ${device.status}%');
    } else if (device.type == 'thermostat') {
      return Text('Temperature: ${device.status}°F');
    }
    return Text('Status: ${device.status}');
  }

  void _navigateToAddDevice(BuildContext context) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => DeviceForm(),
      ),
    ).then((result) {
      if (result != null) {
        //TODO: Change to use the api/devicesRoutes instead of directly calling it.
        http
            .post(
                Uri.parse(
                    'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/Devices'),
                headers: {'Content-Type': 'application/json'},
                body: jsonEncode(result))
            .then((response) {
          if (response.statusCode == 201) {
            fetchDevices();
          } else {
            print('Error: ${response.statusCode}');
          }
        }).catchError(print);
      }
    });
  }

  void _navigateToEditDevice(BuildContext context, Device initialData) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => DeviceForm(
          initialData: initialData,
        ),
      ),
    ).then((device) {
      if (device != null) {
        //TODO: Change to use the api/devicesRoutes instead of directly calling it.
        http
            .put(
                Uri.parse(
                    'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/Devices/${initialData.id}'),
                headers: {'Content-Type': 'application/json'},
                body: jsonEncode(device))
            .then((response) {
          if (response.statusCode == 204) {
            fetchDevices();
          } else {
            print('Error: ${response.statusCode}');
          }
        }).catchError(print);
      }
    });
  }
}

//temporarily here, remove once implemented
DeviceForm({Device? initialData}) {}
