import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/api/api_service.dart';
import 'package:smarter_sleep/app/models/device.dart';

import 'package:smarter_sleep/app/screens/ScheduleFormScreen.dart';
import 'package:smarter_sleep/app/models/device_schedule.dart';

class DeviceSchedulePage extends StatefulWidget {
  final Device device;

  const DeviceSchedulePage({super.key, required this.device});

  @override
  State<DeviceSchedulePage> createState() => _DeviceSchedulePageState();
}

class _DeviceSchedulePageState extends State<DeviceSchedulePage> {
  List<DeviceSchedule> schedules = [];

  @override
  void initState() {
    super.initState();
    fetchDeviceSchedules(widget.device.id!);
  }

  Future<void> fetchDeviceSchedules(int deviceId) async {
    dynamic fetchedSchedules = await ApiService.get('api/DeviceSettings');
    if (fetchedSchedules != null) {
      final now = DateTime.now();
      List<DeviceSchedule> deviceSchedules = fetchedSchedules
          .where((scheduleData) =>
              scheduleData['deviceId'] == deviceId &&
              DateTime.parse(scheduleData['scheduledTime']).isAfter(now))
          .map<DeviceSchedule>((scheduleData) {
        return DeviceSchedule.fromJson(scheduleData);
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
      return Text('Next Alarm: ${schedule.scheduledTime}');
    } else if (widget.device.type == 'light') {
      return Text('Set brightness to ${schedule.settings!['Brightness']}%');
    } else if (widget.device.type == 'thermostat') {
      return Text('Set temperature to ${schedule.settings!['Temperature']}°F');
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
                if (schedule.id != null) {
                  _deleteSchedule(schedule.id!);
                }
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  /// Dropped due to conflicts with sleep setting generation.
  Future<void> _navigateToAddSchedule(BuildContext context) async {
    DeviceSchedule? schedule = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => ScheduleForm(device: widget.device),
      ),
    );
    if (schedule != null) {
      /*
       * List sleepSchedules = fetchSleepSchedules()
       * sleepSchedules.foreach(schedule) =>
       *  if device_setting.scheduled_time < schedule.scheduled_wake and device_setting.scheduled_time > schedule.scheduled_wake + 1 day then
       *    device_setting.sleep_settings_id = schedule.id
       * 
       * if !device_setting.sleep_settings_id then
       *  device_setting.sleep_settings_id = createNewSleepSetting(device_setting.scheduled_time);
      */
      //Does it make sense for every device setting after scheduled_wake goes to the next one
      ApiService.post('api/DeviceSettings', schedule.toJson()).then(
        (response) {
          if (response != null) {
            fetchDeviceSchedules(widget.device.id!);
          }
        },
      );
    }
  }

  void _deleteSchedule(int id) {
    ApiService.delete('api/DeviceSettings/$id').then(
      (response) {
        if (response != null) {
          fetchDeviceSchedules(widget.device.id!);
        }
      },
    );
  }

  Future<void> _navigateToEditSchedule(
      BuildContext context, DeviceSchedule initialData) async {
    DeviceSchedule? schedule = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (context) => ScheduleForm(
          device: widget.device,
          initialData: initialData,
        ),
      ),
    );
    print("Test");
    if (schedule != null) {
      schedule.id = initialData.id;
      schedule.sleepSettingId = initialData.sleepSettingId;

      ApiService.put('api/DeviceSettings/${initialData.id}', schedule.toJson())
          .then(
        (response) {
          if (response != null) {
            fetchDeviceSchedules(widget.device.id!);
          }
        },
      );
    }
  }
}
