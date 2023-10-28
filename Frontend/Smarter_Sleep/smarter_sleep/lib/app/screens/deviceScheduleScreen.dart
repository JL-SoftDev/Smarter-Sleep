import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';

class DeviceSchedulePage extends StatefulWidget {
  final Device device;

  const DeviceSchedulePage({super.key, required this.device});

  @override
  _DeviceSchedulePageState createState() => _DeviceSchedulePageState();
}

class DeviceSchedule {
  final int id;
  final int deviceId;
  final int sleepSettingId;
  final DateTime scheduledTime;
  final Map<String, dynamic> settings;

  DeviceSchedule(this.id, this.deviceId, this.sleepSettingId,
      this.scheduledTime, this.settings);
}

class _DeviceSchedulePageState extends State<DeviceSchedulePage> {
  List<DeviceSchedule> schedules = [];

  @override
  void initState() {
    super.initState();
    // Fetch and display the schedule for the specific device
    fetchDeviceSchedules(widget.device.id);
  }

  Future<void> fetchDeviceSchedules(int deviceId) async {
    print('Device ID: $deviceId');
    final response = await http.get(Uri.parse(
        'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/DeviceSettings'));

    if (response.statusCode == 200) {
      final List<dynamic> data = json.decode(response.body);
      setState(() {
        schedules = data
            .where((scheduleData) => scheduleData['deviceId'] == deviceId)
            .map((scheduleData) {
          return DeviceSchedule(
            scheduleData['id'],
            scheduleData['deviceId'],
            scheduleData['sleepSettingId'],
            DateTime.parse(scheduleData['scheduledTime']),
            json.decode(scheduleData['settings']),
          );
        }).toList();
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('${widget.device.name} Schedule'),
      ),
      body: schedules.isEmpty
          ? const Center(
              child: Text('The device has nothing planned'),
            )
          : ListView.builder(
              itemCount: schedules.length,
              itemBuilder: (context, index) {
                final schedule = schedules[index];
                return ListTile(
                  title: Text('Trigger at ${schedule.scheduledTime.toLocal()}'),
                  subtitle: _buildSettingsWidget(schedule),
                  trailing: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      IconButton(
                        icon: const Icon(Icons.edit),
                        onPressed: () {
                          _editSchedule(schedule);
                        },
                      ),
                      IconButton(
                        icon: const Icon(Icons.delete),
                        onPressed: () {
                          _confirmDeleteSchedule(schedule);
                        },
                      ),
                    ],
                  ),
                );
              },
            ),
    );
  }

  Widget _buildSettingsWidget(DeviceSchedule schedule) {
    if (widget.device.type == 'alarm') {
      return Text('Next Alarm: ${schedule.settings['NextAlarm']}');
    } else if (widget.device.type == 'light') {
      return Text('Set brightness to ${schedule.settings['Brightness']}%');
    } else if (widget.device.type == 'thermostat') {
      return Text('Set temperature to ${schedule.settings['Temperature']}°F');
    }
    return Text('Settings: ${schedule.settings.toString()}');
  }

  void _confirmDeleteSchedule(DeviceSchedule schedule) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Delete Schedule'),
          content: const Text('Are you sure you want to delete this schedule?'),
          actions: <Widget>[
            TextButton(
              child: const Text('Cancel'),
              onPressed: () {
                Navigator.of(context).pop();
              },
            ),
            TextButton(
              child: const Text('Delete'),
              onPressed: () {
                // Implement the deletion logic here
                _deleteSchedule(schedule);
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  void _deleteSchedule(DeviceSchedule schedule) {
    //TODO: Delete the schedule
  }

  void _editSchedule(DeviceSchedule schedule) {
    //TODO: Create page to modify the schedule
  }
}
