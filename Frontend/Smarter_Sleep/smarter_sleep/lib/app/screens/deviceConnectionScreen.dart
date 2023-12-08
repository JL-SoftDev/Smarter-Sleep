//import 'dart:html';

import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
import 'package:smarter_sleep/app/models/device.dart';
import 'package:smarter_sleep/app/models/device_schedule.dart';
import 'package:smarter_sleep/app/models/sleep_schedule.dart';
import 'package:smarter_sleep/app/screens/deviceScheduleScreen.dart';
import 'package:smarter_sleep/main.dart';

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
  GlobalServices _globalServices = GlobalServices();

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

      //Generate a list of device IDs
      var deviceIds = fetchedDevices.map((e) => e.id!).toList();

      dynamic fetchedSchedules = await ApiService.get('api/DeviceSettings');
      if (fetchedSchedules != null) {
        List<DeviceSchedule> deviceSchedules = fetchedSchedules
            .where(
                (scheduleData) => deviceIds.contains(scheduleData['deviceId']))
            .map<DeviceSchedule>((scheduleData) {
          return DeviceSchedule.fromJson(scheduleData);
        }).toList();

        for (int i = 0; i < fetchedDevices.length; i++) {
          var device = fetchedDevices[i];
          List<DeviceSchedule> schedules;
          //Find last schedule change for each schedule device (either before last time or before)
          switch (device.type) {
            case "alarm":
              schedules = deviceSchedules
                  .where((schedule) =>
                      schedule.deviceId == device.id &&
                      schedule.scheduledTime
                          .isAfter(_globalServices.currentTime))
                  .toList();
              break;
            default:
              schedules = deviceSchedules
                  .where((schedule) =>
                      schedule.deviceId == device.id &&
                      schedule.scheduledTime
                          .isBefore(_globalServices.currentTime))
                  .toList();
              break;
          }
          //Sort by scheduled time
          schedules.sort((a, b) => a.scheduledTime.compareTo(b.scheduledTime));

          //Update status
          if (schedules.isNotEmpty) {
            switch (device.type) {
              case "alarm":
                var latestSchedule = schedules.first;
                var newValue = latestSchedule.scheduledTime;
                device.status = "${newValue.toIso8601String()}";
                break;
              case "light":
                var latestSchedule = schedules.last;
                var newValue = latestSchedule.settings?["Brightness"];
                device.status = "${newValue}";
                break;
              case "thermostat":
                var latestSchedule = schedules.last;
                var newValue = latestSchedule.settings?["Temperature"];
                device.status = "${newValue}";
                break;
              default:
                break;
            }
            //Update device in database
            await ApiService.put('api/Devices/${device.id}', device);
          }
        }
      }
      /*

      */

      setState(() {
        devices = fetchedDevices;
      });
    }
  }

  Future<void> updateDevices() async {
    //Iterate over devices
    //Update based on sleepSetttings.deviceSettings
    //Update status to database
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
          onDelete: () {
            _deleteDevice(initialData.id!);
          },
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

  Future<void> _deleteDevice(int id) async {
    ApiService.delete('api/Devices/$id').then(
      (_) {
        fetchDevices();
      },
    );
  }
}
