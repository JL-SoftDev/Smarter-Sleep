import 'package:flutter/material.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:intl/intl.dart';
import 'package:smarter_sleep/app/models/device.dart';
import 'package:smarter_sleep/app/models/device_schedule.dart';

import 'package:smarter_sleep/app/models/sleep_review.dart';
import 'package:smarter_sleep/app/utils/color_utils.dart';

class SleepInsightScreen extends StatefulWidget {
  final SleepReview review;

  const SleepInsightScreen({super.key, required this.review});

  @override
  State<SleepInsightScreen> createState() => _SleepInsightScreenState();
}

class _SleepInsightScreenState extends State<SleepInsightScreen> {
  late int totalSleep;
  late int remDuration;
  late int lightDuration;
  late int deepDuration;

  @override
  void initState() {
    super.initState();

    totalSleep = widget.review.survey.sleepDuration;
    List<String> sleepStages = widget.review.wearableLog.hypnogram.split('');

    remDuration = sleepStages.where((stage) => stage == '3').length * 5;
    lightDuration = sleepStages.where((stage) => stage == '2').length * 5;
    deepDuration = sleepStages.where((stage) => stage == '1').length * 5;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
            'Sleep Insight - ${DateFormat('MMM dd, yyyy').format(widget.review.createdAt)}'),
      ),
      body: ListView(
        children: [
          const SizedBox(
            height: 25,
          ),
          Text(
            "Smarter Sleep Score",
            textAlign: TextAlign.center,
            style: TextStyle(
              color: ColorUtils.getScoreColor(widget.review.smarterSleepScore),
              fontSize: 18,
              fontWeight: FontWeight.bold,
            ),
          ),
          const SizedBox(height: 12),
          Text(
            widget.review.smarterSleepScore.toString(),
            textAlign: TextAlign.center,
            style: TextStyle(
                fontSize: 35,
                fontWeight: FontWeight.bold,
                color:
                    ColorUtils.getScoreColor(widget.review.smarterSleepScore)),
          ),
          Padding(
            padding: const EdgeInsets.fromLTRB(32, 0, 32, 12),
            child: Column(
              children: [
                ListTile(
                  contentPadding: const EdgeInsets.symmetric(
                      horizontal: 0.0, vertical: 0.0),
                  visualDensity:
                      const VisualDensity(horizontal: 0, vertical: -4),
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text(
                        'Total Sleep',
                        style: TextStyle(fontWeight: FontWeight.bold),
                      ),
                      Text(
                        '${(totalSleep / 60).floor()}h ${(totalSleep % 60).toString().padLeft(2, "0")}m',
                        style: const TextStyle(fontWeight: FontWeight.bold),
                      ),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: totalSleep / 480,
                  color: ColorUtils.getScoreColor((totalSleep / 4.8).round(),
                      start: Colors.red, end: Colors.blue),
                  backgroundColor: ColorUtils.getScoreColor(
                      (totalSleep / 4.8).round(),
                      start: Colors.red[100]!,
                      end: Colors.blue[100]!),
                  minHeight: 4,
                ),
                ListTile(
                  dense: true,
                  contentPadding: const EdgeInsets.symmetric(
                      horizontal: 0.0, vertical: 0.0),
                  visualDensity:
                      const VisualDensity(horizontal: 0, vertical: -4),
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text('REM Duration'),
                      Text(
                          '${(remDuration / 60).floor()}h ${(remDuration % 60).toString().padLeft(2, "0")}m'),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: remDuration / (480 * .25),
                  color: ColorUtils.getScoreColor(
                      (remDuration / (4.8 * .25)).round(),
                      start: Colors.red,
                      end: Colors.blue),
                  backgroundColor: ColorUtils.getScoreColor(
                      (remDuration / (4.8 * .25)).round(),
                      start: Colors.red[100]!,
                      end: Colors.blue[100]!),
                  minHeight: 2,
                ),
                ListTile(
                  dense: true,
                  contentPadding: const EdgeInsets.symmetric(
                      horizontal: 0.0, vertical: 0.0),
                  visualDensity:
                      const VisualDensity(horizontal: 0, vertical: -4),
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text('Light Sleep Duration (N1/N2)'),
                      Text(
                          '${(lightDuration / 60).floor()}h ${(lightDuration % 60).toString().padLeft(2, "0")}m'),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: lightDuration / (480 * .55),
                  color: ColorUtils.getScoreColor(
                      (lightDuration / (4.8 * .55)).round(),
                      start: Colors.red,
                      end: Colors.blue),
                  backgroundColor: ColorUtils.getScoreColor(
                      (lightDuration / (4.8 * .55)).round(),
                      start: Colors.red[100]!,
                      end: Colors.blue[100]!),
                  minHeight: 2,
                ),
                ListTile(
                  dense: true,
                  contentPadding: const EdgeInsets.symmetric(
                      horizontal: 0.0, vertical: 0.0),
                  visualDensity:
                      const VisualDensity(horizontal: 0, vertical: -4),
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text('Deep Sleep Duration (N3)'),
                      Text(
                          '${(deepDuration / 60).floor()}h ${(deepDuration % 60).toString().padLeft(2, "0")}m'),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: deepDuration / (480 * .20),
                  color: ColorUtils.getScoreColor(
                      (deepDuration / (4.8 * .20)).round(),
                      start: Colors.red,
                      end: Colors.blue),
                  backgroundColor: ColorUtils.getScoreColor(
                      (deepDuration / (4.8 * .20)).round(),
                      start: Colors.red[100]!,
                      end: Colors.blue[100]!),
                  minHeight: 2,
                ),
                const SizedBox(height: 15.0),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                  children: [
                    ElevatedButton.icon(
                      onPressed: () {
                        _listDataPopup("Collected Survey Data",
                            widget.review.survey.toJson());
                      },
                      icon: const Icon(Icons.description),
                      label: const Text('Survey Input'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.black26,
                      ),
                    ),
                    ElevatedButton.icon(
                      onPressed: () {
                        _listDataPopup("Collected Wearable Data",
                            widget.review.wearableLog.toJson());
                      },
                      icon: const Icon(Icons.accessibility),
                      label: const Text('Wearable Data'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.black26,
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
          Text(
            'Sleep Stages',
            style: TextStyle(
              color: Colors.blue[800],
              fontSize: 18,
              fontWeight: FontWeight.bold,
              letterSpacing: 2,
            ),
            textAlign: TextAlign.center,
          ),
          Padding(
            padding: const EdgeInsets.only(right: 16, left: 6, top: 15),
            child: Center(
              child: SizedBox(
                height: MediaQuery.of(context).size.height * 0.28,
                width: MediaQuery.of(context).size.width * 0.9,
                child: LineChart(mainData()),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(10.0),
            child: ElevatedButton.icon(
              onPressed: () {
                if (widget.review.sleepSchedule != null &&
                    widget.review.sleepSchedule!.deviceSchedules != null &&
                    widget.review.sleepSchedule!.deviceSchedules!.isNotEmpty) {
                  _listDeviceSchedules(
                      widget.review.sleepSchedule!.deviceSchedules!);
                } else {
                  showDialog(
                    context: context,
                    builder: (BuildContext context) {
                      return AlertDialog(
                        title: const Text("No Applied Settings"),
                        content: const Text(
                            "Smarter Sleep did not apply any device settings for this day"),
                        actions: [
                          TextButton(
                            onPressed: () {
                              Navigator.of(context).pop();
                            },
                            child: const Text('OK'),
                          ),
                        ],
                      );
                    },
                  );
                }
              },
              icon: const Icon(Icons.history),
              label: const Text("Explore Day's Device Adjustments"),
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.blue,
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _listDataPopup(String name, Map<String, dynamic> data) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(name),
          content: Container(
            constraints: BoxConstraints(
                maxHeight: MediaQuery.of(context).size.height * 0.50),
            child: SingleChildScrollView(
              child: Column(
                children: data.keys.map((key) {
                  return ListTile(
                    dense: true,
                    title: Text(key),
                    subtitle: Text('${data[key]}'),
                  );
                }).toList(),
              ),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
              },
              child: const Text('Close'),
            ),
          ],
        );
      },
    );
  }

  void _listDeviceSchedules(List<DeviceSchedule> deviceSchedules) {
    deviceSchedules.sort((a, b) => a.scheduledTime.compareTo(b.scheduledTime));

    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text("Device Schedules"),
          content: SizedBox(
            width: MediaQuery.of(context).size.width,
            height: MediaQuery.of(context).size.height * 0.5,
            child: ListView.builder(
              itemCount: deviceSchedules.length,
              itemBuilder: (context, index) {
                DeviceSchedule deviceSchedule = deviceSchedules[index];
                Device device = deviceSchedule.device ??
                    Device(userId: '', name: "Unknown Device", type: "?");

                String settingString = deviceSchedule.settings.toString();
                switch (device.type) {
                  case "light":
                    settingString =
                        "Set to ${deviceSchedule.settings!['Brightness']}%";
                    break;
                  case "thermostat":
                    settingString =
                        "Set to ${deviceSchedule.settings!['Temperature']}°F";
                    break;
                  case "alarm":
                    settingString = "Activate";
                }

                return ListTile(
                  leading: Icon(device.getDeviceIcon()),
                  dense: true,
                  horizontalTitleGap: 0.0,
                  contentPadding: const EdgeInsets.all(0.0),
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Expanded(
                        child: Text(device.name),
                      ),
                      Text(settingString),
                    ],
                  ),
                  subtitle: Text("${deviceSchedule.scheduledTime.toLocal()}"),
                );
              },
            ),
          ),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
              },
              child: const Text('Close'),
            ),
          ],
        );
      },
    );
  }

  Widget bottomTitleWidgets(double value, TitleMeta meta) {
    const style = TextStyle(
      fontWeight: FontWeight.bold,
      fontStyle: FontStyle.italic,
      fontSize: 14,
    );
    Widget text;
    switch (value.toInt()) {
      case 0:
        text = const Text('0H', style: style);
        break;
      case 23:
        text = const Text('2H', style: style);
        break;
      case 47:
        text = const Text('4H', style: style);
        break;
      case 71:
        text = const Text('6H', style: style);
        break;
      case 95:
        text = const Text('8H', style: style);
        break;
      default:
        text = const Text('', style: style);
        break;
    }

    return SideTitleWidget(
      axisSide: meta.axisSide,
      child: text,
    );
  }

  Widget leftTitleWidgets(double value, TitleMeta meta) {
    const style = TextStyle(
      fontWeight: FontWeight.bold,
      fontSize: 12,
    );
    String text;
    switch (value.round()) {
      case 1:
        text = 'DEEP';
        break;
      case 2:
        text = 'LIGHT';
        break;
      case 3:
        text = 'REM';
        break;
      case 4:
        text = 'AWAKE';
        break;
      default:
        return Container();
    }

    return Text(text, style: style, textAlign: TextAlign.left);
  }

  LineChartData mainData() {
    return LineChartData(
      gridData: const FlGridData(show: false),
      titlesData: FlTitlesData(
        show: true,
        rightTitles: const AxisTitles(
          sideTitles: SideTitles(showTitles: false),
        ),
        topTitles: const AxisTitles(
          sideTitles: SideTitles(showTitles: false),
        ),
        bottomTitles: AxisTitles(
          sideTitles: SideTitles(
            showTitles: true,
            reservedSize: 30,
            interval: 1,
            getTitlesWidget: bottomTitleWidgets,
          ),
        ),
        leftTitles: AxisTitles(
          sideTitles: SideTitles(
            showTitles: true,
            interval: 1,
            getTitlesWidget: leftTitleWidgets,
            reservedSize: 45,
          ),
        ),
      ),
      minY: .49,
      maxY: 4.5,
      lineBarsData: [
        LineChartBarData(
          spots: widget.review.wearableLog.hypnogram
              .split('')
              .asMap()
              .map((key, char) =>
                  MapEntry(key, FlSpot(key.toDouble(), double.parse(char))))
              .values
              .toList(),
          isCurved: false,
          isStrokeCapRound: true,
          color: Colors.blue[800],
          dotData: const FlDotData(show: false),
          belowBarData: BarAreaData(show: false),
        ),
      ],
      borderData: FlBorderData(
        show: true,
        border: Border.all(
          color: const Color(0xff37434d),
          width: 1,
        ),
      ),
    );
  }
}
