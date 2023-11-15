import 'package:flutter/material.dart';

class SurveyForm extends StatefulWidget {
  //final int? trackedTime;

  //const SurveyForm({super.key, this.trackedTime});
  const SurveyForm({super.key});

  @override
  State<SurveyForm> createState() => _SurveyFormState();
}

class _SurveyFormState extends State<SurveyForm> {
  int _restedRating = 10;
  final List<bool> _selectedWakePreference = <bool>[false, true, false];
  final List<bool> _selectedTemperature = <bool>[false, true, false];
  bool _lights = false;
  bool _sleepTime = false;
  //int sleepDuration = 0;

  /* Can be used if wished to pass sleep duration to be editable.
  @override
  void initState() {
    if (widget.trackedTime != null) {
      sleepDuration = widget.trackedTime!;
    }
    super.initState();
  }*/

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Survey Form'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
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
              'Would you like to wake earlier, later, or the same time next week?',
              textAlign: TextAlign.center,
              style: TextStyle(
                fontSize: 14,
                color: Colors.black,
              ),
            ),
            const SizedBox(height: 8),
            ToggleButtons(
              direction: Axis.horizontal,
              onPressed: (int index) {
                setState(() {
                  for (int i = 0; i < _selectedWakePreference.length; i++) {
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
              children: const [Text('Earlier'), Text("Same"), Text("Later")],
            ),
            const SizedBox(height: 16),
            const Text(
              'Were you too hot, too cold, or just right?',
              textAlign: TextAlign.center,
              style: TextStyle(
                fontSize: 14,
                color: Colors.black,
              ),
            ),
            const SizedBox(height: 8),
            ToggleButtons(
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
              children: const [Text('Cold'), Text("Niether"), Text("Hot")],
            ),
            const SizedBox(height: 16),
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
            const SizedBox(height: 16),
            const Text(
              'Would you like to try to go to sleep earlier next week?',
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
            const SizedBox(height: 20),
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context, _saveSurvey());
              },
              child: const Text('Submit Survey'),
            ),
          ],
        ),
      ),
    );
  }

  Map<String, dynamic> _saveSurvey() {
    return {};
  }
}
