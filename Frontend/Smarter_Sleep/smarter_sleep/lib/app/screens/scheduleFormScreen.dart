import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/screens/deviceConnectionScreen.dart';
import 'package:smarter_sleep/app/screens/deviceScheduleScreen.dart';

class ScheduleForm extends StatefulWidget {
  final Device device;
  final DeviceSchedule? initialData;

  const ScheduleForm({super.key, required this.device, this.initialData});

  @override
  _ScheduleFormState createState() => _ScheduleFormState();
}

class _ScheduleFormState extends State<ScheduleForm> {
  DateTime selectedDate = DateTime.now();
  TimeOfDay selectedTime = TimeOfDay.now();
  int brightnessValue = 100;
  int temperatureValue = 72;

  @override
  void initState() {
    super.initState();
    if (widget.initialData != null) {
      setState(() {
        selectedDate = widget.initialData!.scheduledTime;
        selectedTime =
            TimeOfDay.fromDateTime(widget.initialData!.scheduledTime);

        final settings = widget.initialData!.settings;

        if (widget.device.type == 'light') {
          brightnessValue = settings['Brightness'];
        } else if (widget.device.type == 'thermostat') {
          temperatureValue = settings['Temperature'];
        }
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Schedule Device Setting'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: <Widget>[
            Row(
              children: <Widget>[
                Expanded(
                  child: TextFormField(
                    decoration:
                        const InputDecoration(labelText: 'Scheduled Date'),
                    readOnly: true,
                    controller: TextEditingController(
                      text: selectedDate.toLocal().toString().split(' ')[0],
                    ),
                    onTap: () async {
                      final DateTime? pickedDate = await showDatePicker(
                        context: context,
                        initialDate: selectedDate,
                        firstDate: DateTime.now(),
                        lastDate: DateTime(2101),
                      );

                      if (pickedDate != null && pickedDate != selectedDate) {
                        setState(() {
                          selectedDate = pickedDate;
                        });
                      }
                    },
                  ),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: TextFormField(
                    decoration:
                        const InputDecoration(labelText: 'Scheduled Time'),
                    readOnly: true,
                    controller: TextEditingController(
                      text: selectedTime.format(context),
                    ),
                    onTap: () async {
                      final TimeOfDay? pickedTime = await showTimePicker(
                        context: context,
                        initialTime: selectedTime,
                      );

                      if (pickedTime != null && pickedTime != selectedTime) {
                        setState(() {
                          selectedTime = pickedTime;
                        });
                      }
                    },
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            if (widget.device.type == 'light')
              Column(
                children: <Widget>[
                  Text('Brightness: $brightnessValue'),
                  Slider(
                    value: brightnessValue.toDouble(),
                    onChanged: (value) {
                      setState(() {
                        brightnessValue = value.toInt();
                      });
                    },
                    min: 0,
                    max: 100,
                  ),
                ],
              ),
            if (widget.device.type == 'thermostat')
              Column(
                children: <Widget>[
                  Text('Temperature: $temperatureValue°F'),
                  Slider(
                    value: temperatureValue.toDouble(),
                    onChanged: (value) {
                      setState(() {
                        temperatureValue = value.toInt();
                      });
                    },
                    min: 60,
                    max: 80,
                  ),
                ],
              ),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context, _saveSchedule());
              },
              child: const Text('Save Schedule'),
            ),
          ],
        ),
      ),
    );
  }

  Map<String, dynamic> _saveSchedule() {
    Map<String, dynamic> settings = {};

    if (widget.device.type == 'alarm') {
      settings = {
        'NextAlarm': DateTime(
          selectedDate.year,
          selectedDate.month,
          selectedDate.day,
          selectedTime.hour,
          selectedTime.minute,
        ).toIso8601String(),
      };
    } else if (widget.device.type == 'light') {
      settings = {'Brightness': brightnessValue};
    } else if (widget.device.type == 'thermostat') {
      settings = {'Temperature': temperatureValue};
    }

    return {
      'deviceId': widget.device.id,
      'scheduledTime': DateTime(
        selectedDate.year,
        selectedDate.month,
        selectedDate.day,
        selectedTime.hour,
        selectedTime.minute,
      ).toIso8601String(),
      'settings': jsonEncode(settings)
    };
  }
}
