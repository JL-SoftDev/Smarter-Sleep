import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:smarter_sleep/main.dart';

class SurveyForm extends StatefulWidget {
  final int? trackedTime;

  const SurveyForm({super.key, this.trackedTime});

  @override
  State<SurveyForm> createState() => _SurveyFormState();
}

class _SurveyFormState extends State<SurveyForm> {
  final GlobalServices _globalServices = GlobalServices();

  int _restedRating = 10;
  final List<bool> _selectedWakePreference = <bool>[false, true, false];
  final List<bool> _selectedTemperature = <bool>[false, true, false];
  bool _lights = false;
  bool _sleepTime = false;
  bool _ateLate = false;
  int _sleepDuration = 0;
  bool _overrideDuration = false;
  final _newDurationController = TextEditingController();

  @override
  void initState() {
    if (widget.trackedTime != null) {
      _sleepDuration = widget.trackedTime!;
    }
    super.initState();
  }

  @override
  void dispose() {
    _newDurationController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Survey Form'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(children: [
          Expanded(
            child: ListView(
              children: [
                const Text(
                  'How rested did you feel this morning?',
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 14,
                    color: Colors.black,
                  ),
                ),
                Slider(
                  value: _restedRating.toDouble(),
                  onChanged: (double value) {
                    setState(() {
                      _restedRating = value.round();
                    });
                  },
                  min: 0,
                  max: 10,
                  divisions: 10,
                  label: _restedRating.toString(),
                ),
                const SizedBox(height: 16),
                const Text(
                  'Wake earlier, later, or same time next week?',
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 14,
                    color: Colors.black,
                  ),
                ),
                const SizedBox(height: 8),
                Center(
                  child: ToggleButtons(
                    direction: Axis.horizontal,
                    onPressed: (int index) {
                      setState(() {
                        for (int i = 0;
                            i < _selectedWakePreference.length;
                            i++) {
                          _selectedWakePreference[i] = i == index;
                        }
                      });
                    },
                    borderRadius: const BorderRadius.all(Radius.circular(8)),
                    selectedBorderColor: Colors.blue[700],
                    selectedColor: Colors.white,
                    fillColor: Colors.blue[200],
                    color: Colors.blue[400],
                    constraints: const BoxConstraints(
                      minHeight: 30.0,
                      minWidth: 100.0,
                    ),
                    isSelected: _selectedWakePreference,
                    children: const [
                      Text('Earlier'),
                      Text("Same"),
                      Text("Later")
                    ],
                  ),
                ),
                const SizedBox(height: 16),
                const Text(
                  'Were you too hot, too cold, or niether?',
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 14,
                    color: Colors.black,
                  ),
                ),
                const SizedBox(height: 8),
                Center(
                  child: ToggleButtons(
                    direction: Axis.horizontal,
                    onPressed: (int index) {
                      setState(() {
                        for (int i = 0; i < _selectedTemperature.length; i++) {
                          _selectedTemperature[i] = i == index;
                        }
                      });
                    },
                    borderRadius: const BorderRadius.all(Radius.circular(8)),
                    selectedBorderColor: Colors.blue[700],
                    selectedColor: Colors.white,
                    fillColor: Colors.blue[200],
                    color: Colors.blue[400],
                    constraints: const BoxConstraints(
                      minHeight: 30.0,
                      minWidth: 100.0,
                    ),
                    isSelected: _selectedTemperature,
                    children: const [
                      Text('Cold'),
                      Text("Niether"),
                      Text("Hot")
                    ],
                  ),
                ),
                const SizedBox(height: 8),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const Text(
                      'Did your lights bother you before your alarm?',
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        fontSize: 14,
                        color: Colors.black,
                      ),
                    ),
                    Switch(
                      value: _lights,
                      onChanged: (bool value) {
                        setState(() {
                          _lights = value;
                        });
                      },
                    ),
                  ],
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const Text(
                      'Do you want to sleep earlier next week?',
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        fontSize: 14,
                        color: Colors.black,
                      ),
                    ),
                    Switch(
                      value: _sleepTime,
                      onChanged: (bool value) {
                        setState(() {
                          _sleepTime = value;
                        });
                      },
                    ),
                  ],
                ),
                Row(mainAxisAlignment: MainAxisAlignment.center, children: [
                  const Text(
                    'Did you eat within 90 minutes before sleeping?',
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontSize: 14,
                      color: Colors.black,
                    ),
                  ),
                  Switch(
                    value: _ateLate,
                    onChanged: (bool value) {
                      setState(() {
                        _ateLate = value;
                      });
                    },
                  ),
                ]),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      'Override Collected Sleep Duration: $_sleepDuration',
                      textAlign: TextAlign.center,
                      style: const TextStyle(
                        fontSize: 14,
                        color: Colors.black,
                      ),
                    ),
                    Switch(
                      value: _overrideDuration,
                      onChanged: (bool value) {
                        setState(() {
                          _overrideDuration = value;
                        });
                      },
                    ),
                  ],
                ),
                if (_overrideDuration)
                  Container(
                    padding: const EdgeInsets.fromLTRB(75, 0, 75, 10),
                    child: TextField(
                      decoration: const InputDecoration(
                          hintText: 'Time Slept in Minutes',
                          labelText: 'Input Actual Sleep Duration'),
                      controller: _newDurationController,
                      keyboardType: TextInputType.number,
                    ),
                  ),
              ],
            ),
          ),
          ElevatedButton(
            onPressed: () {
              Navigator.pop(context, _saveSurvey());
            },
            child: const Text('Submit Survey'),
          ),
        ]),
      ),
    );
  }

  Map<String, dynamic> _saveSurvey() {
    return {
      "createdAt": _globalServices.currentTime,
      "sleepQuality": _restedRating,
      "wakePreference": _selectedWakePreference.indexOf(true),
      "temperaturePreference": _selectedTemperature.indexOf(true),
      "lightsDisturbance": _lights,
      "sleepEarlier": _sleepTime,
      "ateLate": _ateLate,
      "sleepDuration":
          _overrideDuration ? _newDurationController.text : _sleepDuration,
      "surveyDate": DateFormat('yyyy-MM-dd').format(_globalServices.currentTime)
    };
  }
}
