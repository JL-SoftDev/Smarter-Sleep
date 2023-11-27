import 'package:flutter/material.dart';
import 'package:fl_chart/fl_chart.dart';

import 'package:smarter_sleep/app/models/sleep_review.dart';
import 'package:smarter_sleep/app/utils/color_utils.dart';

class SleepInsightScreen extends StatefulWidget {
  final SleepReview review;

  const SleepInsightScreen({super.key, required this.review});

  @override
  State<SleepInsightScreen> createState() => _SleepInsightScreenState();
}

class _SleepInsightScreenState extends State<SleepInsightScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Sleep Insight - ${widget.review.survey.surveyDate}'),
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
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text('Total Sleep'),
                      Text(
                          '${(widget.review.survey.sleepDuration / 60).floor()}h ${(widget.review.survey.sleepDuration % 60).toString().padLeft(2, "0")}m'),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: widget.review.survey.sleepDuration / 480,
                  color: ColorUtils.getScoreColor(
                      (widget.review.survey.sleepDuration / 4.8).round(),
                      start: Colors.red,
                      end: Colors.blue),
                  backgroundColor: ColorUtils.getScoreColor(
                      (widget.review.survey.sleepDuration / 4.8).round(),
                      start: Colors.red[100]!,
                      end: Colors.blue[100]!),
                  minHeight: 2,
                ),
                ListTile(
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text('Total Sleep'),
                      Text('5h 32m'),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: 0.5,
                  minHeight: 2,
                ),
                ListTile(
                  title: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text('Total Sleep'),
                      Text('5h 32m'),
                    ],
                  ),
                ),
                LinearProgressIndicator(
                  value: 0.5,
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
