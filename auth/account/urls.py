from django.urls import path

from . import views

urlpatterns = [
    path('authenticate/', views.authenticate_view, name="authenticate"),
    path('tokens/', views.character_list, name="tokens"),
    path('session/', views.session_create, name="session-map")
]
