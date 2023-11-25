class WearableLog {
  final DateTime sleepStart;
  final DateTime sleepEnd;
  final String hypnogram;
  final int sleepScore;
  final String sleepDate;

  WearableLog({
    required this.sleepStart,
    required this.sleepEnd,
    required this.hypnogram,
    required this.sleepScore,
    required this.sleepDate,
  });

  factory WearableLog.fromJson(Map<String, dynamic> json) {
    return WearableLog(
      sleepStart: DateTime.parse(json['sleepStart']),
      sleepEnd: DateTime.parse(json['sleepEnd']),
      hypnogram: json['hypnogram'],
      sleepScore: json['sleepScore'],
      sleepDate: json['sleepDate'],
    );
  }

  Map<String, dynamic> toJson() => <String, dynamic>{
        'sleepStart': sleepStart.toIso8601String(),
        'sleepEnd': sleepEnd.toIso8601String(),
        'hypnogram': hypnogram,
        'sleepScore': sleepScore,
        'sleepDate': sleepDate,
      };
}
