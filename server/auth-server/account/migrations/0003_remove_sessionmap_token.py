# Generated by Django 4.1.4 on 2023-03-16 18:45

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('account', '0002_rename_character_sessionmap_token'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='sessionmap',
            name='token',
        ),
    ]