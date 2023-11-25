import 'dart:async';
import 'dart:convert';
import 'dart:math';
import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

import 'package:smarter_sleep/app/models/user_challenge.dart';
import 'package:smarter_sleep/app/models/sleep_review.dart';

import 'surveyFormScreen.dart';
import '../appFrame.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  List<UserChallenge> userChallenges = [];
  bool isSleeping = false;
  late Stopwatch sleepTime;
  late Timer updateTime;
  int? _sleepScore;
  late String userId;

  @override
  void initState() {
    super.initState();

    sleepTime = Stopwatch();
    updateTime = Timer.periodic(const Duration(minutes: 1), (timer) {
      setState(() {});
    });

    _initializeUser();
  }

  @override
  void dispose() {
    updateTime.cancel();
    super.dispose();
  }

  Future<void> _initializeUser() async {
    final user = await Amplify.Auth.getCurrentUser();

    //TODO: Fetch challenges from API(likely need to define a new api to return challenge progress)
    List<UserChallenge> fetchedChallenges = [
      UserChallenge(
        challengeName: "Sleep On Schedule",
        startDate: DateTime.now(),
        expireDate: DateTime.now().add(const Duration(days: 4)),
        userSelected: true,
        completionPercentage: (Random().nextInt(4) + 1) / 5,
      ),
      UserChallenge(
        challengeName: "No Lights 1 Hour",
        startDate: DateTime.now(),
        expireDate: DateTime.now().add(const Duration(days: 14)),
        userSelected: true,
        completionPercentage: (Random().nextInt(4) + 1) / 5,
      ),
      UserChallenge(
        challengeName: "No Eating Before Bed",
        startDate: DateTime.now(),
        expireDate: DateTime.now().add(const Duration(days: 2)),
        userSelected: true,
        completionPercentage: (Random().nextInt(4) + 1) / 5,
      ),
    ];

    final response = await http.get(Uri.parse(
        'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/SleepReviews'));

    int fetchedSleepScore = 0;
    if (response.statusCode == 200) {
      List<dynamic> body = json.decode(response.body);
      List<SleepReview> reviews = body
          .map((json) => SleepReview.fromJson(json))
          .where((review) => review.userId == user.userId)
          .toList();
      if (reviews.isNotEmpty) {
        SleepReview lastReview =
            reviews.reduce((a, b) => a.createdAt.isAfter(b.createdAt) ? a : b);
        fetchedSleepScore = lastReview.smarterSleepScore;
      }
    }

    setState(() {
      userId = user.userId;
      userChallenges = fetchedChallenges;
      _sleepScore = fetchedSleepScore;
    });
  }

  //TODO: Convert to WearableLog?
  Future<Map<String, dynamic>> fetchWearableData(bool goodData) async {
    //TODO: Fetch the generated wearable data from the Web API using the goodData var
    DateTime now = DateTime.now();

    DateTime lastNight =
        now.subtract(const Duration(days: 1)).add(const Duration(hours: 21));
    DateTime wakeTime = lastNight.add(const Duration(hours: 8));
    Map<String, dynamic> data = {
      "sleepStart": lastNight.toIso8601String(),
      "sleepEnd": wakeTime.toIso8601String(),
      "hypnogram":
          "443432222211222333321112222222222111133333322221111223333333333223222233222111223333333333332224",
      "sleepScore": 100,
      "sleepDate": DateFormat('yyyy-MM-dd').format(lastNight)
    };

    return data;
  }

  Future<void> _popupReview(SleepReview review) async {
    return showDialog(
        context: context,
        builder: (BuildContext context) {
          return AlertDialog(
            title: const Text('Sleep Review Data'),
            content: SingleChildScrollView(
                child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('Created At: ${review.createdAt.toString()}'),
                Text('Smarter Sleep Score: ${review.smarterSleepScore}'),
                SizedBox(height: 10),
                Text('Survey Details:'),
                Text('  Sleep Quality: ${review.survey.sleepQuality}'),
                Text('  Wake Preference: ${review.survey.wakePreference}'),
                Text(
                    '  Temperature Preference: ${review.survey.temperaturePreference}'),
                Text(
                    '  Lights Disturbance: ${review.survey.lightsDisturbance}'),
                Text('  Sleep Earlier: ${review.survey.sleepEarlier}'),
                Text('  Ate Late: ${review.survey.ateLate}'),
                Text('  Sleep Duration: ${review.survey.sleepDuration}'),
                Text('  Survey Date: ${review.survey.surveyDate}'),
                SizedBox(height: 10),
                Text('Wearable Log Details:'),
                Text(
                    '  Sleep Start: ${review.wearableLog.sleepStart.toString()}'),
                Text('  Sleep End: ${review.wearableLog.sleepEnd.toString()}'),
                Text('  Hypnogram: ${review.wearableLog.hypnogram}'),
                Text('  Sleep Score: ${review.wearableLog.sleepScore}'),
                Text('  Sleep Date: ${review.wearableLog.sleepDate}'),
              ],
            )),
            actions: <Widget>[
              TextButton(
                onPressed: () => Navigator.pop(context, 'OK'),
                child: const Text('OK'),
              ),
            ],
          );
        });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Smarter Sleep Home"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      body: Center(
        child: Column(
          children: <Widget>[
            FractionallySizedBox(
              widthFactor: 1.0,
              child: Container(
                height: MediaQuery.of(context).size.height * 0.28,
                color: isSleeping ? Colors.purple[900] : Colors.blue[800],
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: <Widget>[
                    Text(
                      isSleeping ? "Sleeping" : "Smarter Sleep Score",
                      style: const TextStyle(
                        color: Colors.white,
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 40),
                    isSleeping
                        ? Text(
                            '${sleepTime.elapsed.inHours.toString().padLeft(2, "0")}H:${(sleepTime.elapsed.inMinutes % 60).toString().padLeft(2, "0")}M',
                            style: const TextStyle(
                                fontSize: 40, color: Colors.white))
                        : Stack(
                            alignment: Alignment.center,
                            children: <Widget>[
                              Transform.scale(
                                scale: 2.5,
                                child: CircularProgressIndicator(
                                  value: (_sleepScore ?? 100) / 100,
                                  valueColor:
                                      const AlwaysStoppedAnimation<Color>(
                                          Colors.white),
                                ),
                              ),
                              Text(_sleepScore?.toString() ?? '--',
                                  style: const TextStyle(
                                      fontSize: 40, color: Colors.white)),
                            ],
                          ),
                  ],
                ),
              ),
            ),
            const SizedBox(height: 20),
            const Text(
              "User Challenges",
              style: TextStyle(
                fontSize: 24,
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(height: 10),
            ChallengeList(userChallenges: userChallenges),
            Expanded(
                child: Align(
              alignment: FractionalOffset.bottomCenter,
              child: Padding(
                  padding: const EdgeInsets.only(bottom: 20),
                  child: SizedBox(
                    width: 290,
                    height: 50,
                    child: FilledButton(
                        onPressed: () => toggleSleep(),
                        style: FilledButton.styleFrom(
                            backgroundColor: const Color(0xff6750a4)),
                        child: Text(
                          isSleeping ? "End Sleep" : "Start Sleeping",
                          style: const TextStyle(
                            fontSize: 30,
                          ),
                        )),
                  )),
            )),
          ],
        ),
      ),
    );
  }

  Future<void> submitSleepData(survey, wearableData) async {
    if (survey != null && wearableData != null) {
      Map<String, dynamic> payload = {
        "survey": survey,
        "wearableData": wearableData
      };
      //TODO: Change to use the api/devicesRoutes instead of directly calling it.
      http
          .post(
              Uri.parse(
                  'http://ec2-54-87-139-255.compute-1.amazonaws.com/api/SleepReviews/GenerateReview/$userId'),
              headers: {'Content-Type': 'application/json'},
              body: jsonEncode(payload))
          .then((response) {
        if (response.statusCode == 201) {
          _popupReview(SleepReview.fromJson(json.decode(response.body)));
        } else {
          print(response.body);
          print('Error: ${response.statusCode}');
        }
      }).catchError(print);
    }
  }

  void toggleSleep() {
    setState(() {
      isSleeping = !isSleeping;
      if (isSleeping) {
        sleepTime.start();
      } else {
        sleepTime.stop();
        int minutesSlept = sleepTime.elapsed.inMinutes;
        sleepTime.reset();
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => SurveyForm(trackedTime: minutesSlept),
          ),
        ).then((survey) async {
          //survey['sleepDuration'] = minutesSlept;
          var wearableData = await fetchWearableData(false);
          submitSleepData(survey, wearableData);
        });
      }
    });
  }
}

class ChallengeList extends StatelessWidget {
  final List<UserChallenge> userChallenges;

  final List<Color> predefinedColors = [
    Colors.indigo.shade400,
    Colors.lightGreen,
    Colors.deepPurple.shade400,
  ];

  int colorIndex = 0;

  ChallengeList({super.key, required this.userChallenges});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: userChallenges.map((userChallenge) {
        final Duration remainingTime =
            userChallenge.expireDate.difference(DateTime.now());

        Color color = predefinedColors[colorIndex];
        colorIndex = (colorIndex + 1) % predefinedColors.length;

        return Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            children: <Widget>[
              Stack(
                children: [
                  LinearProgressIndicator(
                    borderRadius: BorderRadius.circular(4),
                    backgroundColor: Colors.blueGrey[100],
                    color: color,
                    value: userChallenge.completionPercentage,
                    minHeight: 50,
                  ),
                  Positioned(
                    left: 8,
                    top: 5,
                    child: Text(
                      userChallenge.challengeName,
                      style: const TextStyle(
                        fontSize: 22,
                        color: Colors.black,
                      ),
                    ),
                  ),
                  Positioned(
                    left: 8,
                    top: 30,
                    child: Text(
                      'Expires in ${remainingTime.inDays} days',
                      style: const TextStyle(
                        fontSize: 12,
                        color: Colors.black,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                  Positioned(
                    right: 10,
                    top: 13,
                    child: Text(
                      "${(userChallenge.completionPercentage * 100).toInt()}%",
                      style: const TextStyle(
                        fontSize: 20,
                        color: Colors.black,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ],
              ),
            ],
          ),
        );
      }).toList(),
    );
  }
}
