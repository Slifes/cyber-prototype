# Generated by Django 4.1.4 on 2022-12-26 22:40

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Authentication',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('address', models.CharField(db_index=True, max_length=64, verbose_name='Address')),
                ('token', models.CharField(max_length=100, verbose_name='Token')),
                ('expire_at', models.DateTimeField(verbose_name='Expire at')),
                ('valid', models.BooleanField(default=True, verbose_name='Is Valid?')),
            ],
            options={
                'verbose_name': 'Authentication',
                'verbose_name_plural': 'Authentications',
            },
        ),
        migrations.CreateModel(
            name='Token',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('owner', models.CharField(db_index=True, max_length=64, verbose_name='Address')),
                ('token_id', models.PositiveIntegerField(db_index=True, unique=True, verbose_name='TokenID')),
            ],
            options={
                'verbose_name': 'Token',
                'verbose_name_plural': 'Tokens',
            },
        ),
        migrations.CreateModel(
            name='SessionMap',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('address', models.CharField(db_index=True, max_length=64, verbose_name='Address')),
                ('auth_token', models.CharField(max_length=100, verbose_name='Token')),
                ('expire_at', models.DateTimeField(verbose_name='Expire at')),
                ('authenticated', models.BooleanField(default=False, verbose_name='Authenticated')),
                ('character', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='account.token')),
            ],
            options={
                'verbose_name': 'Session',
                'verbose_name_plural': 'Sessions',
            },
        ),
    ]
