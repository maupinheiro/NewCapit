

interface AutocompleteSuggestion {
    value: string;
    data: any;
}

interface AutocompleteInstance {
   
    clear(): void;
    clearCache(): void;
    disable(): void;
    enable(): void;
    hide(): void;
    dispose(): void;
}
