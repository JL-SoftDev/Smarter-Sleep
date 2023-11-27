import 'package:flutter/material.dart';
import 'package:fl_chart/fl_chart.dart';

import 'package:smarter_sleep/app/models/sleep_review.dart';

class SleepInsightScreen extends StatefulWidget {
  final SleepReview review;

  const SleepInsightScreen({super.key, required this.review});

  @override
  State<SleepInsightScreen> createState() => _SleepInsightScreenState();
}

class _SleepInsightScreenState extends State<SleepInsightScreen> {
  int _sleepScore = 100;
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
              color: Colors.blue[800],
              fontSize: 18,
              fontWeight: FontWeight.bold,
            ),
          ),
          const SizedBox(height: 12),
          Text(
            _sleepScore?.toString() ?? '--',
            textAlign: TextAlign.center,
            style: TextStyle(
                fontSize: 35,
                fontWeight: FontWeight.bold,
                color: Colors.blue[800]),
          ),
          const Padding(
            padding: const EdgeInsets.all(32.0),
            child: Column(
              children: [
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
              ],
            ),
          ),
          AspectRatio(
            aspectRatio: 1.23,
            child: Column(
              children: [
                SizedBox(height: 50),
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
                Expanded(
                  child: Padding(
                    padding: const EdgeInsets.only(right: 16, left: 6),
                    child: Center(
                      child: SizedBox(
                        height: MediaQuery.of(context).size.height * 0.28,
                        width: MediaQuery.of(context).size.width * 0.9,
                        child: LineChart(mainData()),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
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
