from django.contrib import admin

from .models import Character, Inventory, Equipment

admin.site.register(Equipment)
admin.site.register(Inventory)
admin.site.register(Character)