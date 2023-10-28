import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:smarter_sleep/app/screens/ScheduleFormScreen.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';
import 'dart:convert';

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
    fetchDeviceSchedules(widget.device.id);
  }

  Future<void> fetchDeviceSchedules(int deviceId) async {
    final response = await http.get(Uri.parse(
        'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/DeviceSettings'));

    if (response.statusCode == 200) {
      final List<dynamic> data = json.decode(response.body);
      final now = DateTime.now();

      List<DeviceSchedule> deviceSchedules = data
          .where((scheduleData) =>
              scheduleData['deviceId'] == deviceId &&
              DateTime.parse(scheduleData['scheduledTime']).isAfter(now))
          .map((scheduleData) {
        return DeviceSchedule(
          scheduleData['id'],
          scheduleData['deviceId'],
          scheduleData['sleepSettingId'],
          DateTime.parse(scheduleData['scheduledTime']),
          json.decode(scheduleData['settings']),
        );
      }).toList();

      // Sort settings by scheduledTime ascending(soonest schedules first)
      deviceSchedules
          .sort((a, b) => a.scheduledTime.compareTo(b.scheduledTime));

      setState(() {
        schedules = deviceSchedules;
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
                          _navigateToEditSchedule(context, schedule);
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
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          _navigateToAddSchedule(context);
        },
        child: const Icon(Icons.add),
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
                _deleteSchedule(schedule.id);
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  void _navigateToAddSchedule(BuildContext context) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => ScheduleForm(device: widget.device),
      ),
    ).then((result) {
      if (result != null) {
        //TODO: Post the setting to the database and refresh the page
        //Requires the setting to include a sleepSettingId, which is not currently implemented.
      }
    });
  }

  void _deleteSchedule(int id) {
    http
        .delete(Uri.parse(
            'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/DeviceSettings/${id}'))
        .then((response) {
      if (response.statusCode == 204) {
        fetchDeviceSchedules(widget.device.id);
      } else {
        print('Error: ${response.statusCode}');
      }
    }).catchError(print);
  }

  void _navigateToEditSchedule(
      BuildContext context, DeviceSchedule initialData) {
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => ScheduleForm(
          device: widget.device,
          initialData: initialData,
        ),
      ),
    ).then((schedule) {
      if (schedule != null) {
        schedule['id'] = initialData.id;
        schedule['sleepSettingId'] = initialData.sleepSettingId;
        http
            .put(
                Uri.parse(
                    'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/DeviceSettings/${initialData.id}'),
                headers: {'Content-Type': 'application/json'},
                body: jsonEncode(schedule))
            .then((response) {
          if (response.statusCode == 204) {
            fetchDeviceSchedules(widget.device.id);
          } else {
            print('Error: ${response.statusCode}');
          }
        }).catchError(print);
      }
    });
  }
}
