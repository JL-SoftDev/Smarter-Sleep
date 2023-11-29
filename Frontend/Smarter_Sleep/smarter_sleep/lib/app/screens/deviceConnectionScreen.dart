import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
import 'package:smarter_sleep/app/models/device.dart';
import 'package:smarter_sleep/app/screens/deviceScheduleScreen.dart';

import 'deviceFormScreen.dart';
import '../appFrame.dart';

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
    dynamic response = await ApiService.get('api/Devices');
    if (response != null) {
      final user = await Amplify.Auth.getCurrentUser();
      final userId = user.userId;

      List<Device> fetchedDevices = response
          .where((deviceData) => deviceData['userId'] == userId)
          .map<Device>((deviceData) {
        return Device.fromJson(deviceData);
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
          final formattedTime = "${nextAlarm.hour}h ${nextAlarm.minute}m";
          return Text('Next Alarm: $formattedTime');
        }
      }
      return Text('Next Alarm\n ${device.status}');
    } else if (device.type == 'light') {
      return Text('Brightness: ${device.status}%');
    } else if (device.type == 'thermostat') {
      return Text('Temperature: ${device.status}°F');
    }
    return Text('Status: ${device.status}');
  }

  Future<void> _navigateToAddDevice(BuildContext context) async {
    Device? device = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => const DeviceForm(),
      ),
    );
    if (device != null) {
      final user = await Amplify.Auth.getCurrentUser();

      device.userId = user.userId;

      await ApiService.post('api/Devices', device);
      fetchDevices();
    }
  }

  Future<void> _navigateToEditDevice(
      BuildContext context, Device initialData) async {
    Device? newData = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => DeviceForm(
          initialData: initialData,
        ),
      ),
    );
    if (newData != null) {
      final user = await Amplify.Auth.getCurrentUser();

      newData.userId = user.userId;
      newData.id = initialData.id;

      await ApiService.put('api/Devices/${initialData.id}', newData);
      fetchDevices();
    }
  }
}
