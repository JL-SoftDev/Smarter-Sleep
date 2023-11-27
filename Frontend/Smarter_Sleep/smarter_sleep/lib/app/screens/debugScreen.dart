import 'package:flutter/material.dart';
import 'package:smarter_sleep/main.dart';
import '../appFrame.dart';

class DebugScreen extends StatefulWidget {
  const DebugScreen({super.key});

  @override
  State<DebugScreen> createState() => _DebugScreenState();
}

//TODO: Allow user to set current DateTime throughout device

class _DebugScreenState extends State<DebugScreen> {
  final GlobalServices _globalServices = GlobalServices();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("Debug"),
          actions: [
            IconButton(
              icon: const Icon(Icons.account_circle),
              onPressed: () {
                mainNavigatorKey.currentState!.pushNamed("/account");
              },
            ),
          ],
        ),
        body: ListView(
          children: [
            //Show time
            Center(
                child: Text(
              "Current Time: ${_globalServices.currentTime}",
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            )),
            //Change Current Time button
            TextButton(
                onPressed: _onChangeTimePressed,
                child: Text("Change Current Time")),
          ],
        ));
  }

  _onChangeTimePressed() async {
    //TODO - Popup allowing the user to set the time
    var newDate = await selectDate(context, _globalServices.currentTime);
    var newTime = await selectTime(
        context, TimeOfDay.fromDateTime(_globalServices.currentTime));
    if (newTime == null || newDate == null) return;

    setState(() {
      //Update the datetime with the new time
      _globalServices.currentTime = DateTime(
        newDate.year,
        newDate.month,
        newDate.day,
        newTime.hour,
        newTime.minute,
      );
    });
  }

  Future<DateTime?> selectDate(
      BuildContext context, DateTime initialDate) async {
    final DateTime? pickedDate = await showDatePicker(
      context: context,
      initialDate: initialDate,
      firstDate: DateTime(2000), // Adjust the range as needed
      lastDate: DateTime(2030),
    );
    return pickedDate;
  }

  Future<TimeOfDay?> selectTime(
      BuildContext context, TimeOfDay initialTime) async {
    final TimeOfDay? pickedTime = await showTimePicker(
      context: context,
      initialTime: initialTime,
    );
    return pickedTime;
  }
}
