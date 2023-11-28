import 'package:flutter/material.dart';
import 'package:smarter_sleep/app/models/device.dart';

class DeviceForm extends StatefulWidget {
  final Device? initialData;

  const DeviceForm({super.key, this.initialData});

  @override
  State<DeviceForm> createState() => _DeviceFormState();
}

class _DeviceFormState extends State<DeviceForm> {
  final name = TextEditingController();
  String selectedType = 'light';
  DateTime selectedDate = DateTime.now();
  TimeOfDay selectedTime = TimeOfDay.now();
  int brightnessValue = 100;
  int temperatureValue = 72;

  @override
  void initState() {
    super.initState();
    if (widget.initialData != null) {
      final device = widget.initialData!;
      selectedType = device.type;
      name.text = device.name;
      if (selectedType == 'alarm') {
        selectedDate = DateTime.parse(device.status!);
        selectedTime = TimeOfDay.fromDateTime(DateTime.parse(device.status!));
      } else if (selectedType == 'light') {
        brightnessValue = int.parse(device.status.toString());
      } else if (selectedType == 'thermostat') {
        temperatureValue = int.parse(device.status.toString());
      }
    }
  }

  @override
  void dispose() {
    name.dispose();
    super.dispose();
  }

  Future<void> _showDatePicker() async {
    final DateTime? pickedDate = await showDatePicker(
      context: context,
      initialDate:
          selectedDate.isBefore(DateTime.now()) ? DateTime.now() : selectedDate,
      firstDate: DateTime.now(),
      lastDate: DateTime(2101),
    );

    if (pickedDate != null) {
      setState(() {
        selectedDate = pickedDate;
      });
    }
  }

  Future<void> _showTimePicker() async {
    final TimeOfDay? pickedTime = await showTimePicker(
      context: context,
      initialTime: selectedTime,
    );

    if (pickedTime != null) {
      setState(() {
        selectedTime = pickedTime;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Device Form'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: <Widget>[
            TextFormField(
              decoration: const InputDecoration(labelText: 'Device Name'),
              controller: name,
            ),
            const SizedBox(height: 16),
            DropdownButton<String>(
              value: selectedType,
              onChanged: (String? newValue) {
                setState(() {
                  selectedType = newValue!;
                });
              },
              items: ['alarm', 'light', 'thermostat'].map((String type) {
                return DropdownMenuItem<String>(
                  value: type,
                  child: Row(
                    children: <Widget>[
                      if (type == 'alarm') const Icon(Icons.alarm),
                      if (type == 'light') const Icon(Icons.lightbulb),
                      if (type == 'thermostat') const Icon(Icons.thermostat),
                      const SizedBox(width: 8),
                      Text(type),
                    ],
                  ),
                );
              }).toList(),
            ),
            const SizedBox(height: 16),
            if (selectedType == 'alarm')
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
                      onTap: () => _showDatePicker(),
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
                      onTap: () => _showTimePicker(),
                    ),
                  ),
                ],
              ),
            if (selectedType == 'light')
              Column(
                children: <Widget>[
                  Text('Brightness: $brightnessValue'),
                  Slider(
                    value: brightnessValue.toDouble(),
                    onChanged: (double value) {
                      setState(() {
                        brightnessValue = value.round();
                      });
                    },
                    min: 0,
                    max: 100,
                  ),
                ],
              ),
            if (selectedType == 'thermostat')
              Column(
                children: <Widget>[
                  Text('Temperature: $temperatureValue°F'),
                  Slider(
                    value: temperatureValue.toDouble(),
                    onChanged: (double value) {
                      setState(() {
                        temperatureValue = value.round();
                      });
                    },
                    min: 60,
                    max: 80,
                  ),
                ],
              ),
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context, _saveDeviceSettings());
              },
              child: const Text('Save Device Settings'),
            ),
          ],
        ),
      ),
    );
  }

  Device _saveDeviceSettings() {
    String settings = '0';

    if (selectedType == 'alarm') {
      settings = DateTime(
        selectedDate.year,
        selectedDate.month,
        selectedDate.day,
        selectedTime.hour,
        selectedTime.minute,
      ).toIso8601String();
    } else if (selectedType == 'light') {
      settings = brightnessValue.toString();
    } else if (selectedType == 'thermostat') {
      settings = temperatureValue.toString();
    }

    return Device(
      userId: "",
      name: name.text,
      type: selectedType,
      status: settings,
    );
  }
}
