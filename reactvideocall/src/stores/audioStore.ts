import { makeAutoObservable, reaction } from "mobx";

export default class AudioStore{
    audio = new Audio('/FacebookMessengerCall.mp3');
    playing = false;

    constructor(){
        makeAutoObservable(this);
        reaction(
            () => this.playing,
            () => {
                this.playing ? this.audio.play() : this.audio.pause();
            }
        )
        this.audio.addEventListener('ended', () => this.setPlaying(false));        
    }

    toogle = ()=>{
        this.playing = !this.playing;
    }

    setPlaying = (playing :boolean)=>{
        this.playing = playing;
    }

}