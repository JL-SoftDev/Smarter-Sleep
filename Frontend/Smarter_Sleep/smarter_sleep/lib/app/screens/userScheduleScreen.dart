import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import '../appFrame.dart';

class UserScheduleScreen extends StatefulWidget {
  const UserScheduleScreen({super.key});

  @override
  State<UserScheduleScreen> createState() => _UserScheduleScreenState();
}

class _UserScheduleScreenState extends State<UserScheduleScreen> {
  List<CustomSchedule> schedule = [];

  @override
  void initState() {
    super.initState();
    fetchUserSchedule();
  }

  Future<void> fetchUserSchedule() async {
    //TODO: Fetch schedules from API, expect the data shown below
    List<dynamic> body = [
      {
        "UserId": "73b11e71-e9ac-4fb1-9b9b-f7b667d1e45a",
        "DayOfWeek": 1,
        "WakeTime": "08:00:00"
      }
    ];

    //TODO: Filter for logged in user through amplify
    List<CustomSchedule> fetchedSchedule =
        body.map((json) => CustomSchedule.fromJson(json)).toList();

    //TODO: Account for not all DayOfWeeks being set(i.e. fetchedSchedule is empty or only has some days)
    print("Time:${fetchedSchedule[0].wakeTime.toString()}");

    setState(() {
      schedule = fetchedSchedule;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("User Schedule"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      //TODO: Design the page layout and prompt for user to input times
      //TODO: Add some way for the user to submit schedules, send a post request to the api with the new data
      body: const Center(child: Text("Not yet implemented")),
    );
  }
}

//Can be moved to app/models however only used on this screen
class CustomSchedule {
  final String userId;
  final int dayOfWeek;
  final TimeOfDay wakeTime;

  CustomSchedule({
    required this.userId,
    required this.dayOfWeek,
    required this.wakeTime,
  });

  factory CustomSchedule.fromJson(Map<String, dynamic> json) {
    return CustomSchedule(
      userId: json['UserId'],
      dayOfWeek: json['DayOfWeek'] as int,
      wakeTime: TimeOfDay.fromDateTime(
          DateTime.parse("1970-01-01 ${json['WakeTime']}")),
    );
  }

  Map<String, dynamic> toJson() => <String, dynamic>{
        'UserId': userId,
        'DayOfWeek': dayOfWeek,
        'WakeTime':
            "${wakeTime.hour}:${wakeTime.minute.toString().padLeft(2, '0')}:00",
      };
}
