INSERT INTO challenge (Id, Name, Description, Reward)
VALUES 
    (1, 'Zzz Zoom', 'Zoom into bed earlier for a night of uninterrupted Zzz''s, setting the stage for a morning full of possibilities.', 100),
    (2, 'Dreamy Eight', 'Dive into a dreamy world with the challenge of securing a solid eight hours of sleep for ultimate rejuvenation.', 100),
    (3, 'Midnight Munch Ban', 'Avoid the temptation of a snack before bed, go 90 minutes without eating prior to sleep.', 100),
    (4, 'Dynamic Dream Duo', 'Interact with the Smarter Sleep app everyday for two weeks straight.', 100),
    (5, 'Seamless Sleep Automation', 'Allow Smarter Sleep to schedule smart home devices without overriding any options', 100),
    (6, 'Sunrise Scheduler', 'Specify your non-negotiable wake-up times, allowing us to tailor your sleep routine to meet your daily commitments', 100)
ON CONFLICT (Id) DO UPDATE SET 
    Name = EXCLUDED.Name,
    Description = EXCLUDED.Description,
    Reward = EXCLUDED.Reward;